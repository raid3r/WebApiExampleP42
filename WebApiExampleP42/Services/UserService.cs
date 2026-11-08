using WebApiExampleP42.Models;

namespace WebApiExampleP42.Services;


/*

Контроллер
Дозволено:




Заборонено:
- містити логіку бізнесу у контролері

Сервіс








  
 
 */

public class UserService
{
    private readonly IDataProvider<User> dataProvider;

    public UserService(IDataProvider<User> dataProvider)
    {
        this.dataProvider = dataProvider;
    }

    public async Task<ICollection<User>> GetAllAsync()
    {
        return await dataProvider.GetAllAsync();
    }

    public async Task<User> RegisterAsync(User newUser)
    {
        var users = await dataProvider.GetAllAsync();

        // email unique
        if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        

        newUser.Token = Guid.NewGuid().ToString(); // Generate a unique token for the user
        newUser.IsActive = true; // Set the user as active by default
        newUser.RoleId = Roles.User; // Assign the default role (User)

        newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(newUser);
        await dataProvider.SaveAsync(users);
        return newUser;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var users = await dataProvider.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        return user;
    }

    public async Task<User> CreateUserAsync(User newUser)
    {
        var users = await dataProvider.GetAllAsync();

        // email unique
        if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(newUser);
        await dataProvider.SaveAsync(users);
        return newUser;
    }

    public async Task<User?> UpdateUserAsync(User updatedUser)
    {
        var users = await dataProvider.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user == null)
        {
            return null;
        }
        // Check for email uniqueness if the email is being updated
        if (!user.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase) &&
            users.Any(u => u.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }
        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password; // In a real application, ensure to hash the password
        user.Token = updatedUser.Token;
        user.IsActive = updatedUser.IsActive;
        user.RoleId = updatedUser.RoleId;
        await dataProvider.SaveAsync(users);
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var users = await dataProvider.GetAllAsync();
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User?> GetUserByTokenAsync(string token)
    {
        var users = await dataProvider.GetAllAsync();
        return users.FirstOrDefault(u => u.Token == token);
    }
}
