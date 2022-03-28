using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace ElancoGroupB.Models;

public class Service
{
    public static async Task<Purchase> RequestAnalyzeDocumentAsync(string filePath)
    {
        string endpoint = "https://form-recognizer-elanco.cognitiveservices.azure.com/";
        string apiKey = "ad92cf9cf82a49efbbc7b51d7a8bdfe2";
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
                if (field.Content.Length > 0) extractedModel.dataCount++;
                switch (fieldName)
                {
                    case "clinic_name":
                        extractedModel.Clinic.Name = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "clinic_address":
                        extractedModel.Clinic.Address = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "clinic_zip_code":
                        extractedModel.Clinic.Zip = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "clinic_phone_number":
                        extractedModel.Clinic.Phone = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "pet_name":
                        extractedModel.Pet.Name = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "invoice_number":
                        extractedModel.InvoiceNumber = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "total_amount":
                        extractedModel.TotalAmount = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                    case "date_of_service":
                        extractedModel.Date = new Dictionary<string, string?>()
                        {
                            {"value", field.Content},
                            {"confidence", field.Confidence.ToString()},
                        };
                        break;
                }
            }
        }

        return extractedModel;
    }
}