using CareNest_Service_Category.Application.Features.Commands.Create;
using CareNest_Service_Category.Application.Features.Commands.Update;
using CareNest_Service_Category.Domain.Commons.Constant;
using System.Text.RegularExpressions;

namespace CareNest_Service_Category.Application.Exceptions.Validators
{
    public class Validate
    {
        /// <summary>
        /// kiểm tra toàn bộ tạo dịch vụ
        /// </summary>
        /// <param name="command"></param>
        public static void ValidateCreate(CreateCommand command)
        {
            ValidateName(command.Name);
        }

        public static void ValidateUpdate(UpdateCommand command)
        {
            ValidateName(command.Name);
        }
        public static void ValidateName(string? name)
        {
            //-Không được để trống.
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestException(MessageConstant.MissingName);
            }
            //- Giới hạn độ dài(ví dụ 1 - 100 ký tự).
            if (name.Length == 0 || name.Length > 100)
            {
                throw new BadRequestException(MessageConstant.Exceed100CharsName);
            }
            //- Không chứa ký tự đặc biệt (!@#$^*&<>?)
            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9\s]+$"))
            {
                throw new BadRequestException(MessageConstant.SpecialCharacterName);
            }
        }
        /// <summary>
        /// valid id shop
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="BadRequestException"></exception>
        public static void ValidateShopId(string? id)
        {
            // id của shop không được trống
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BadRequestException(MessageConstant.MissingShopId);
            }
        }
    }
}
