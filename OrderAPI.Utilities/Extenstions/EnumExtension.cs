using OrderAPI.Dtos.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderAPI.Utilities.Extenstions
{
    public static class EnumExtension
    {
        public static string GetStringFromProductTypeDtoEnum(this ProductTypeDto productTypeDto)
        {
            if (productTypeDto == ProductTypeDto.photoBook)
                return "photoBook";

            if (productTypeDto == ProductTypeDto.calendar)
                return "calendar";

            if (productTypeDto == ProductTypeDto.canvas)
                return "canvas";

            if (productTypeDto == ProductTypeDto.cards)
                return "cards";

            if (productTypeDto == ProductTypeDto.mug)
                return "mug";

            return "NoProductType";
        }
    }
}
