using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;

namespace ElancoGroupB.Models;

public class Service
{
    private readonly string _apiKey;
    private readonly string _endpoint;
    public Service(string apiKey, string endpoint)
    {
        _apiKey = apiKey;
        _endpoint = endpoint;
        Console.WriteLine(_apiKey);
        Console.WriteLine(_endpoint);
    }

    public async Task<IEnumerable<Item>> RequestReceiptModelAsync(string filePath)
    {
        var credential = new AzureKeyCredential(_apiKey);
        var client = new FormRecognizerClient(new Uri(_endpoint), credential);
        
        using var stream = new FileStream(filePath, FileMode.Open);
        var options = new RecognizeInvoicesOptions() { Locale = "en-US" };

        RecognizeInvoicesOperation operation = await client.StartRecognizeInvoicesAsync(stream, options);
        Response<RecognizedFormCollection> operationResponse = await operation.WaitForCompletionAsync();
        RecognizedFormCollection invoices = operationResponse.Value;
  
        RecognizedForm invoice = invoices.Single();

        List<Item> result = new List<Item>();
        
        if (invoice.Fields.TryGetValue("Items", out FormField itemsField))
        {
            if (itemsField.Value.ValueType == FieldValueType.List)
            {
                foreach (FormField itemField in itemsField.Value.AsList())
                {
                    Console.WriteLine("Item:");
                    Item item = new Item();

                    if (itemField.Value.ValueType == FieldValueType.Dictionary)
                    {
                        IReadOnlyDictionary<string, FormField> itemFields = itemField.Value.AsDictionary();

                        if (itemFields.TryGetValue("Description", out FormField itemDescriptionField))
                        {
                            if (itemDescriptionField.Value.ValueType == FieldValueType.String)
                            {
                                string itemDescription = itemDescriptionField.Value.AsString();

                                Console.WriteLine($"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}");
                                item.Description = new Dictionary<string, string>()
                                {
                                    {"value",itemDescription},
                                    {"confidence", itemDescriptionField.Confidence.ToString()},
                                };
                            }
                        }

                        if (itemFields.TryGetValue("Quantity", out FormField itemQuantityField))
                        {
                            if (itemQuantityField.Value.ValueType == FieldValueType.Float)
                            {
                                float quantityAmount = itemQuantityField.Value.AsFloat();

                                Console.WriteLine($"  Quantity: '{quantityAmount}', with confidence {itemQuantityField.Confidence}");
                                item.Quantity = new Dictionary<string, string>()
                                {
                                    {"value",quantityAmount.ToString()},
                                    {"confidence", itemQuantityField.Confidence.ToString()},
                                };
                            }
                        }

                        if (itemFields.TryGetValue("Amount", out FormField itemAmountField))
                        {
                            if (itemAmountField.Value.ValueType == FieldValueType.Float)
                            {
                                float itemAmount = itemAmountField.Value.AsFloat();

                                Console.WriteLine($"  Amount: '{itemAmount}', with confidence {itemAmountField.Confidence}");
                                item.Amount = new Dictionary<string, string>()
                                {
                                    {"value",itemAmount.ToString()},
                                    {"confidence", itemAmountField.Confidence.ToString()},
                                };
                            }
                        }
                    }

                    result.Add(item);
                }
            }
        }

        return result.ToList();
    }


    public async Task<Purchase> RequestCustom1ModelAsync(string filePath)
    {
       
        var credential = new AzureKeyCredential(_apiKey);
        var client = new DocumentAnalysisClient(new Uri(_endpoint), credential);
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
                Console.WriteLine(fieldName + " : " + field.Content);
                Console.WriteLine(fieldName + " : " + field.Confidence);
                string content = "";
                float confidence = 0;
                if (!String.IsNullOrEmpty(field.Content))
                {
                    content = field.Content;
                }

                if (field.Confidence.HasValue)
                {
                    confidence = (float) field.Confidence;
                }
                
                switch (fieldName)
                {
                    case "clinic_name":
                        extractedModel.ClinicName = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "clinic_address":
                        extractedModel.Address = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "clinic_zip_code":
                        extractedModel.Zip = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "clinic_phone_number":
                        extractedModel.Phone = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "pet_name":
                        extractedModel.PetName = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "invoice_number":
                        extractedModel.InvoiceNumber = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "total_amount":
                        extractedModel.TotalAmount = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                    case "date_of_service":
                        extractedModel.Date = new Dictionary<string, string>()
                        {
                            {"value", content},
                            {"confidence", confidence.ToString()},
                        };
                        break;
                }
            }
        }

        return extractedModel;
    }

    public async Task<Product> RequestproductModelAsync(string filePath)
    {
        var credential = new AzureKeyCredential(_apiKey);
        var client = new DocumentAnalysisClient(new Uri(_endpoint), credential);
        string modelId = "product1"; 

        using var stream = new FileStream(filePath, FileMode.Open);
        
        AnalyzeDocumentOperation operation = await client.StartAnalyzeDocumentAsync(modelId, stream);
        
        await operation.WaitForCompletionAsync();
        
        AnalyzeResult result = operation.Value;
        
        Console.WriteLine($"Document was analyzed with model with ID: {result.ModelId}");
        
        Product product = new Product();

        foreach (AnalyzedDocument document in result.Documents)
        {
            Console.WriteLine($"Document of type: {document.DocType}");
            foreach (KeyValuePair<string, DocumentField> fieldKvp in document.Fields)
            {
                string fieldName = fieldKvp.Key;
                DocumentField field = fieldKvp.Value;
                Console.WriteLine(fieldName);
                Console.WriteLine(field.Content);
                Console.WriteLine(field.Confidence);
                product.Name = new Dictionary<string, string?>()
                {
                    {"value", field.Content ?? ""},
                    {"confidence", field.Confidence.ToString() ?? ""},
                };
            }
        }

        return product;
    }

    
}
