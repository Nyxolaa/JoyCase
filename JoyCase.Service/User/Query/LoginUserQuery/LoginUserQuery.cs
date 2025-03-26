using Azure.Core;
using JoyCase.Application.User.Dto;
using JoyCase.Data;
using JoyCase.Data.Repository;
using JoyCase.Validation;
using MediatR;

namespace JoyCase.Application.User.Query.LoginUserQuery
{
    public class LoginUserQuery : IRequest<Response<LoginUserDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public class LoginUserHandler : IRequestHandler<LoginUserQuery, Response<LoginUserDto>>
        {
            private readonly IRepository<Data.User> _userRepository;
            private readonly IRepository<Role> _roleRepository;
            private readonly IValidationService _validationService;
            public LoginUserHandler(IRepository<Data.User> userRepository, IRepository<Role> roleRepository, IValidationService validationService)
            {
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _validationService = validationService;
            }

            public async Task<Response<LoginUserDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
            {
                var validation = _validationService.Validate(request);
                if (!validation.IsValid)
                {
                    var array = new string[] { "Validasyon Hatasi", string.Join(", ", validation.ValidationResult) };
                    return Response<LoginUserDto>.Failure( array );
                }

                var user = await _userRepository.SelectOneAsync(
                    u => u.Username == request.Username,
                    u => new
                    {
                        u.Id,
                        u.Username,
                        u.Firstname,
                        u.Lastname,
                        Roles = u.Roles.Select(r => new
                        {
                            r.Id,
                            Permissions = r.Permissions.Select(p => new { p.Id, p.Name })
                        }).ToList(),
                        UserPermissions = u.Permissions.Select(p => new { p.Id, p.Name }).ToList()
                    }
                 );

                if (user == null)
                {
                    return Response<LoginUserDto>.Failure(new string[] { "Kullanıcı bulunamadı." });
                }

                // kullanicinin sahip oldugu rollerden gelen tum izinler
                var rolePermissions = user.Roles
                    .SelectMany(r => r.Permissions, (r, p) => new RolePermissionDto { RoleId = r.Id, PermissionId = p.Id, Name = p.Name })
                    .Distinct()
                    .ToList();

                // kullanicinin yetkikleri
                var userPermissions = user.UserPermissions
                    .Select(p => new UserPermissionDto { UserId = user.Id, PermissionId = p.Id, Name = p.Name })
                    .ToList();

                // geri donus icin kullanici dto 
                var response = new LoginUserDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    RoleId = user.Roles.FirstOrDefault()?.Id ?? 0, // eger rol yoksa 0 
                    UserPermissions = userPermissions,
                    RolePermissions = rolePermissions
                };

                return Response<LoginUserDto>.Success(response, "Giriş başarılı.");
            }
        }
    }
}
