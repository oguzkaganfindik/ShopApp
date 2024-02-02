using ShopApp.Business.Dtos;
using ShopApp.Business.Services;
using ShopApp.Data.Entities;
using ShopApp.Data.Repositories;

namespace ShopApp.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        public UserManager(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(UserAddDto userAddDto)
        {


            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == userAddDto.Email.ToLower()).ToList();
            // emailleri eşleşen bütün verileri çekip listeye atadım. eğer eleman sayısı 0 ise bu mailden yok. Kayıt olunabilir. 0'dan farklı ise geriye uyarı mesajı gidecek.

            if (hasMail.Any()) // HasMail.Count != 0 -> eski versiyonu
            {
                // TODO: Geriye uyarı mesajı dön.
            }

            // TODO -> Eşleşen veri yoksa, veritabanına ekleme işlemi yapılmalı.
        }
    }
}
