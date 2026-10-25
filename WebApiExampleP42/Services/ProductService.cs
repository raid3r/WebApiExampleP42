using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class ProductService
{
    private readonly string dataFilePath;

    public ProductService(string dataFilePath)
    {
        this.dataFilePath = dataFilePath;
    }

    private async Task<List<Product>> LoadProductsAsync()
    {
        try
        {
            if (File.Exists(dataFilePath))
            {
                var jsonData = await File.ReadAllTextAsync(dataFilePath);
                return System.Text.Json.JsonSerializer.Deserialize<List<Product>>(jsonData) ?? new List<Product>();
            }
            else
            {
                return new List<Product>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading products: {ex.Message}");
            return new List<Product>();
        }
    }
    private async Task SaveProductsAsync(List<Product> products)
    {
        try
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(products);
            await File.WriteAllTextAsync(dataFilePath, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving products: {ex.Message}");
        }
    }

    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        return await LoadProductsAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var products = await LoadProductsAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        var products = await LoadProductsAsync();
        newProduct.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
        products.Add(newProduct);
        await SaveProductsAsync(products);
        return newProduct;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var products = await LoadProductsAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await SaveProductsAsync(products);
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var products = await LoadProductsAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        products.Remove(product);
        await SaveProductsAsync(products);
        return true;
    }
}
