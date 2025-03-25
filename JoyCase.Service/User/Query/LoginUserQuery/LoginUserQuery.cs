using Azure.Identity;
using JoyCase.Application.User.Dto;
using JoyCase.Data.Repository;
using MediatR;

namespace JoyCase.Application.User.Query.LoginUserQuery
{
    public class LoginUserQuery : IRequest<Response<LoginUserDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public class LoginUserHandler : IRequestHandler<LoginUserQuery, Response<LoginUserDto>>
        {
            private readonly IRepository<Data.User> _repository;
            //private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public LoginUserHandler(IRepository<Data.User> repository
                //, IJwtTokenGenerator jwtTokenGenerator
                )
            {
                _repository = repository;
                //_jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<Response<LoginUserDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _repository.SelectOneAsync(u => u.Username == request.Username, u => new LoginUserDto()
                {
                    UserId = u.Id,
                    Username = u.Username,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    RoleId = u.Roles.FirstOrDefault().Id
                });

                if (user == null)
                {
                    return Response<LoginUserDto>.Failure(new string[] { "Kullanıcı bulunamadı." });
                }

                var response = new LoginUserDto()
                {
                    UserId = user.UserId,
                    Firstname = user.Username,
                    Lastname = user.Lastname,
                    Username = user.Username,
                    RoleId = user.RoleId
                };

                // Şifreyi kontrol et (Hashleme yapıyorsan burada çözmen lazım)
                //if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                //{
                //    return Response<LoginUserDto>.Failure(new string[] { "Şifre hatalı." });
                //}

                // JWT Token oluştur
                //var token = _jwtTokenGenerator.GenerateToken(user);

                return Response<LoginUserDto>.Success(response, "Giriş başarılı.");
            }
        }
    }
}
