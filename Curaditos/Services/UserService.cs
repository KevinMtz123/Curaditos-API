using Curaditos.Infraestructure.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersAsync();
        Task<UserDto> AddUsuario(CreateUserDto dto);
        Task<UserDto?> GetUsuarioById(string id);
        Task<UserDto?> GetUsuarioByEmail(string email);
        Task UpdateUsuario(string id, UpdateUserDto dto);
        Task DeleteUsuario(string id);
        Task<bool> UsuarioExists(string id);
        Task<List<string>> GetUserRoles(string userId);
        Task AddToRole(string userId, AddRoleDto dto);
        Task RemoveFromRole(string userId, string roleName);
    }

    public class UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IUserService
    {
        public async Task<List<UserDto>> GetUsersAsync()
        {
            try
            {
                var usuarios = await userManager.Users
                    .OrderBy(u => u.UserName)
                    .ToListAsync();

                var usuariosDto = new List<UserDto>();

                foreach (var usuario in usuarios)
                {
                    var roles = await userManager.GetRolesAsync(usuario);
                    usuariosDto.Add(MapToDto(usuario, roles.ToList()));
                }

                return usuariosDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto> AddUsuario(CreateUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("La contraseña es requerida");

            if (dto.Password != dto.ConfirmPassword)
                throw new InvalidOperationException("Las contraseñas no coinciden");

            try
            {
                // Validar si el email ya existe
                var existeEmail = await userManager.FindByEmailAsync(dto.Email);
                if (existeEmail != null)
                    throw new InvalidOperationException($"Ya existe un usuario con el email {dto.Email}");

                // Validar si el username ya existe
                var existeUsername = await userManager.FindByNameAsync(dto.UserName);
                if (existeUsername != null)
                    throw new InvalidOperationException($"Ya existe un usuario con el nombre de usuario {dto.UserName}");

                // Mapear DTO a entidad
                var usuario = new ApplicationUser
                {
                    UserName = dto.UserName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    EmailConfirmed = true
                };

                // Crear usuario con Identity
                var result = await userManager.CreateAsync(usuario, dto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al crear usuario: {errors}");
                }

                // Retornar DTO
                var roles = await userManager.GetRolesAsync(usuario);
                return MapToDto(usuario, roles.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto?> GetUsuarioById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío", nameof(id));

            try
            {
                var usuario = await userManager.FindByIdAsync(id);
                if (usuario == null)
                    return null;

                var roles = await userManager.GetRolesAsync(usuario);
                return MapToDto(usuario, roles.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario por ID: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto?> GetUsuarioByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío", nameof(email));

            try
            {
                var usuario = await userManager.FindByEmailAsync(email);
                if (usuario == null)
                    return null;

                var roles = await userManager.GetRolesAsync(usuario);
                return MapToDto(usuario, roles.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuario por email: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateUsuario(string id, UpdateUserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID del usuario no puede estar vacío");

            // Validar contraseñas si se proporciona una nueva
            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                if (dto.NewPassword != dto.ConfirmPassword)
                    throw new InvalidOperationException("Las contraseñas no coinciden");
            }

            try
            {
                var existente = await userManager.FindByIdAsync(id);
                if (existente == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                // Validar si el nuevo email ya existe en otro usuario
                if (existente.Email != dto.Email)
                {
                    var emailEnUso = await userManager.FindByEmailAsync(dto.Email);
                    if (emailEnUso != null && emailEnUso.Id != id)
                        throw new InvalidOperationException($"El email {dto.Email} ya está en uso por otro usuario");
                }

                // Validar si el nuevo username ya existe en otro usuario
                if (existente.UserName != dto.UserName)
                {
                    var usernameEnUso = await userManager.FindByNameAsync(dto.UserName);
                    if (usernameEnUso != null && usernameEnUso.Id != id)
                        throw new InvalidOperationException($"El nombre de usuario {dto.UserName} ya está en uso");
                }

                // Actualizar datos básicos
                existente.UserName = dto.UserName;
                existente.Email = dto.Email;
                existente.PhoneNumber = dto.PhoneNumber;

                // Cambiar contraseña si se proporciona una nueva
                if (!string.IsNullOrWhiteSpace(dto.NewPassword))
                {
                    // Remover contraseña anterior
                    var removeResult = await userManager.RemovePasswordAsync(existente);
                    if (!removeResult.Succeeded)
                    {
                        var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Error al remover contraseña anterior: {errors}");
                    }

                    // Agregar nueva contraseña
                    var addResult = await userManager.AddPasswordAsync(existente, dto.NewPassword);
                    if (!addResult.Succeeded)
                    {
                        var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Error al agregar nueva contraseña: {errors}");
                    }
                }

                // Actualizar usuario
                var updateResult = await userManager.UpdateAsync(existente);
                if (!updateResult.Succeeded)
                {
                    var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al actualizar usuario: {errors}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar usuario: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUsuario(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID no puede estar vacío", nameof(id));

            try
            {
                var usuario = await userManager.FindByIdAsync(id);
                if (usuario == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                var result = await userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al eliminar usuario: {errors}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UsuarioExists(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            try
            {
                var usuario = await userManager.FindByIdAsync(id);
                return usuario != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar existencia de usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("El ID no puede estar vacío", nameof(userId));

            try
            {
                var usuario = await userManager.FindByIdAsync(userId);
                if (usuario == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                var roles = await userManager.GetRolesAsync(usuario);
                return roles.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener roles del usuario: {ex.Message}");
                throw;
            }
        }

        public async Task AddToRole(string userId, AddRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("El ID no puede estar vacío", nameof(userId));

            if (dto == null || string.IsNullOrWhiteSpace(dto.RoleName))
                throw new ArgumentException("El nombre del rol no puede estar vacío");

            try
            {
                var usuario = await userManager.FindByIdAsync(userId);
                if (usuario == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                // Verificar si el rol existe
                var roleExists = await roleManager.RoleExistsAsync(dto.RoleName);
                if (!roleExists)
                    throw new InvalidOperationException($"El rol {dto.RoleName} no existe");

                // Remover roles anteriores
                var currentRoles = await userManager.GetRolesAsync(usuario);
                if (currentRoles.Any())
                {
                    var removeResult = await userManager.RemoveFromRolesAsync(usuario, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
                        throw new InvalidOperationException($"Error al remover roles anteriores: {errors}");
                    }
                }

                // Agregar nuevo rol
                var result = await userManager.AddToRoleAsync(usuario, dto.RoleName);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al agregar rol: {errors}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar rol: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveFromRole(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("El ID no puede estar vacío", nameof(userId));

            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("El nombre del rol no puede estar vacío", nameof(roleName));

            try
            {
                var usuario = await userManager.FindByIdAsync(userId);
                if (usuario == null)
                    throw new InvalidOperationException("Usuario no encontrado");

                var result = await userManager.RemoveFromRoleAsync(usuario, roleName);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Error al remover rol: {errors}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al remover rol: {ex.Message}");
                throw;
            }
        }
       

        #region Mapeo

        private static UserDto MapToDto(ApplicationUser entity, List<string> roles)
        {
            return new UserDto
            {
                Id = entity.Id,
                UserName = entity.UserName ?? string.Empty,
                Email = entity.Email ?? string.Empty,
                PhoneNumber = entity.PhoneNumber,
                EmailConfirmed = entity.EmailConfirmed,
                Roles = roles
            };
        }

        #endregion
    }
}
