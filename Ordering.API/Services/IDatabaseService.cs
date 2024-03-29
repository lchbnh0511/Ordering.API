using Dapper;
using Microsoft.Data.SqlClient;
using Ordering.API.Models.Dtos;
using Ordering.API.Models.RequestModels;

namespace Ordering.API.Services;

public interface IDatabaseService
{
    //-----------------Categories
    List<CategoryDto> GetCategories();
    List<ProductDto> GetProducts();
    ProductDto? GetProductByID(int productID);
    List<ProductDto> GetProducts(int categoryID);
    int Add(CategoryRequestModel requestModel);
    
    CategoryDto? GetCategoryByID(int categoryID);
    int DeleteCategory(int categoryID);

    //Create Orders 
    int InsertCustomer(CustomerRequestModel customerRequest);
    int InsertOrder(int customerId, OrderRequestModel orderRequest);
    int InsertOrderDetail(int orderId, int productId, int quantity, decimal price);
    int Update(int categoryID, CategoryRequestModel requestModel);

}

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;
    public DatabaseService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int Add(CategoryRequestModel requestModel)
    {
        string sql = """
            INSERT INTO Categories(
                CategoryName,
                Description,
                PictureUrl)
            VALUES (@name, @description, @pictureUrl)
            """;
        using var connection = GetConnection();
        var result = connection.Execute(sql, new
        {
            name = requestModel.CategoryName,
            description = requestModel.Description,
            pictureUrl = requestModel.PictureUrl
        });

        return result;
    }

    

    public int DeleteCategory(int categoryID)
    {
        string sql = """
            DELETE Categories
            WHERE CategoryID = @id
            """;

        using var connection = GetConnection();

        var result = connection.Execute(sql, new
        {
            id = categoryID
        });

        return result;
    }
    public List<CategoryDto> GetCategories()
    {
        string sql = """
            SELECT CategoryID, CategoryName
            FROM Categories
            """;

        using var connection = GetConnection();

        return connection.Query<CategoryDto>(sql).ToList();
    }

    public ProductDto? GetProductByID(int productID)
    {
        string sql = """
            SELECT p.*, c.CategoryName, s.CompanyName AS SupplierName
            FROM Products p join Categories c 
            ON p.CategoryID = c.CategoryID join Suppliers s 
            ON p.SupplierID = s.SupplierID
            WHERE p.ProductID = @productID
            """;

        using var connection = GetConnection();

        return connection.Query<ProductDto>(sql, new { productID }).FirstOrDefault();
    }
    public CategoryDto? GetCategoryByID(int categoryID)
    {
        string sql = """
            SELECT *
            FROM Categories
            WHERE CategoryID = @categoryID
            """;
        using var connection = GetConnection();

        return connection.Query<CategoryDto>(sql, new { categoryID }).FirstOrDefault();
    }
    public List<ProductDto> GetProducts(int categoryID)
    {
        string sql = """
            SELECT p.*, c.CategoryName, s.CompanyName AS SupplierName 
            FROM Products p join Categories c 
            ON p.CategoryID = c.CategoryID join Suppliers s 
            ON p.SupplierID = s.SupplierID
            WHERE p.CategoryID = @categoryID
            """;

        using var connection = GetConnection();

        return connection.Query<ProductDto>(sql, new { categoryID }).ToList();
    }

    public List<ProductDto> GetProducts()
    {
        string sql = """
            SELECT p.*, c.CategoryName, s.CompanyName AS SupplierName
            FROM Products p join Categories c 
            ON p.CategoryID = c.CategoryID join Suppliers s 
            ON p.SupplierID = s.SupplierID
            """;

        using var connection = GetConnection();

        return connection.Query<ProductDto>(sql).ToList();
    }

    //Create Order
    public int InsertCustomer(CustomerRequestModel customerRequest)
    {
        string sql = """
            INSERT INTO 
                Customer(ContactTitle, ContactName, Address, Phone, Email, CompanyName, City)
            VALUES 
                (@ContactTitle, @ContactName, @Address, @Phone, @Email, @CompanyName, @City);

            SELECT SCOPE_IDENTITY();
            """;

        using var connection = GetConnection();

        return connection.ExecuteScalar<int>(sql, new
        {
            customerRequest.ContactName,
            customerRequest.Address,
            customerRequest.Phone,
            customerRequest.Email,
            customerRequest.CompanyName,
            customerRequest.ContactTitle,
            customerRequest.City
        });
    }

    public int InsertOrder(int customerID, OrderRequestModel orderRequest)
    {
        string sql = """
            INSERT INTO Orders(CustomerID,OrderDate,ShipAddress,ShipCity)
            VALUES (@customerID, @orderDate, @ShipAddress, @ShipCity)

            SELECT SCOPE_IDENTITY();
            """;

        using var connection = GetConnection();

        return connection.ExecuteScalar<int>(sql, new
        {
            orderRequest.ShipAddress,
            orderRequest.ShipCity,
            customerID,
            orderDate = DateTime.Now
        });
    }

    public int InsertOrderDetail(int orderID, int productID, int quantity, decimal price)
    {
        string sql = """
            INSERT INTO OrderDetails(OrderID,ProductID,Quantity,UnitPrice,DiscountPercentage)
            VALUES (@orderID, @productID, @quantity, @price, 0)

            SELECT SCOPE_IDENTITY();
            """;

        using var connection = GetConnection();

        return connection.ExecuteScalar<int>(sql, new
        {
            orderID,
            productID,
            price,
            quantity
        });
    }

    public int Update(int categoryID, CategoryRequestModel requestModel)
    {
        string sql = """
            UPDATE Categories
            SET 
                CategoryName = @name,
                Description = @description,
                PictureUrl = @pictureUrl
            WHERE CategoryID = @id
            """;

        using var connection = GetConnection();

        var result = connection.Execute(sql, new
        {
            name = requestModel.CategoryName,
            description = requestModel.Description,
            pictureUrl = requestModel.PictureUrl,
            id = categoryID
        });

        return result;
    }

    private SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }


}