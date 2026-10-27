using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;

public class ProductService
{
    private readonly IDataProvider<Product> dataProvider;

    public ProductService(IDataProvider<Product> dataProvider)
    {
        this.dataProvider = dataProvider;
    }

    public async Task<ICollection<Product>> GetAllProductsAsync()
    {
        return await dataProvider.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var products = await dataProvider.GetAllAsync();
        return products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product> AddProductAsync(Product newProduct)
    {
        var products = await dataProvider.GetAllAsync();
        newProduct.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
        products.Add(newProduct);
        await dataProvider.SaveAsync(products);
        return newProduct;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product updatedProduct)
    {
        var products = await dataProvider.GetAllAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        await dataProvider.SaveAsync(products);
        return product;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var products = await dataProvider.GetAllAsync();
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }
        products.Remove(product);
        await dataProvider.SaveAsync(products);
        return true;
    }
}
