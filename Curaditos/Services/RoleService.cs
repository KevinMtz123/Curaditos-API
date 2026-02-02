using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Curaditos.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(string id);
        Task<RoleDto?> GetRoleByNameAsync(string name);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto dto);
        Task<bool> DeleteRoleAsync(string name);
        Task<bool> RoleExistsAsync(string name);
    }
    public class RoleService(RoleManager<IdentityRole> _roleManager) : IRoleService
    {

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .ToListAsync();

            return roles.Select(r => MapToDto(r)).ToList();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID del rol no puede estar vacío", nameof(id));

            var role = await _roleManager.FindByIdAsync(id);

            return role != null ? MapToDto(role) : null;
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del rol no puede estar vacío", nameof(name));

            var role = await _roleManager.FindByNameAsync(name);

            return role != null ? MapToDto(role) : null;
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("El nombre del rol es requerido", nameof(dto.Name));

            // Validar si ya existe
            var existeRol = await _roleManager.RoleExistsAsync(dto.Name);
            if (existeRol)
                throw new InvalidOperationException($"El rol '{dto.Name}' ya existe");

            // Crear el rol
            var role = new IdentityRole { Name = dto.Name };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Error al crear el rol: {errors}");
            }

            return MapToDto(role);
        }

        public async Task<bool> DeleteRoleAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del rol no puede estar vacío", nameof(name));

            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Error al eliminar el rol: {errors}");
            }

            return true;
        }

        public async Task<bool> RoleExistsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return await _roleManager.RoleExistsAsync(name);
        }

        private static RoleDto MapToDto(IdentityRole role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            };
        }
    }
}
