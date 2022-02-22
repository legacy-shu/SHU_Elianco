using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace ElancoGroupB.Models;

public class Service
{
    public static async Task<Purchase> RequestAnalyzeDocumentAsync(string filePath)
    {
        filePath = "sample/Receipt4.png";

        string endpoint = "https://form-recognizer-elanco.cognitiveservices.azure.com/";
        string apiKey = "d89c75fca6d04055bf52c642b54f479c";
        var credential = new AzureKeyCredential(apiKey);
        var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

        string modelId = "custom1"; 

        using var stream = new FileStream(filePath, FileMode.Open);

        AnalyzeDocumentOperation operation = await client.StartAnalyzeDocumentAsync(modelId, stream);

        await operation.WaitForCompletionAsync();

        AnalyzeResult result = operation.Value;

        Console.WriteLine($"Document was analyzed with model with ID: {result.ModelId}");

        Purchase extractedModel = new Purchase();

        foreach (AnalyzedDocument document in result.Documents)
        {

            Console.WriteLine($"Document of type: {document.DocType}");

            foreach (KeyValuePair<string, DocumentField> fieldKvp in document.Fields)
            {
                string fieldName = fieldKvp.Key;
                DocumentField field = fieldKvp.Value;
                // Console.WriteLine($"Field '{fieldName}': ");
                // Console.WriteLine($"  Content: '{field.Content}'");
                // Console.WriteLine($"  Confidence: '{field.Confidence}'");
                switch (fieldName)
                {
                    case "clinic_name":
                        extractedModel.Clinic.Name = field.Content;
                        break;
                    case "clinic_address":
                        extractedModel.Clinic.Address = field.Content;
                        break;
                    case "clinic_zip_code":
                        extractedModel.Clinic.Zip = field.Content;
                        break;
                    case "clinic_phone_number":
                        extractedModel.Clinic.Phone = field.Content;
                        break;
                    case "pet_name":
                        extractedModel.Pet.Name = field.Content;
                        break;
                    case "invoice_number":
                        extractedModel.InvoiceNumber = field.Content;
                        break;
                    case "total_amount":
                        extractedModel.TotalAmount = field.Content;
                        break;
                    case "date_of_service":
                        extractedModel.Date = field.Content;
                        break;
                }
            }
        }

        return extractedModel;
    }
}