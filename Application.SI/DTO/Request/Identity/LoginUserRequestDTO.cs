using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SI.DTO.Request.Identity
{
    public class LoginUserRequestDTO
    {
        [EmailAddress]
        [RegularExpression("[^@ \\t \\r \\n]+@[^@ \\t \\r \\n]+\\.[^@ \\t \\r \\n]+", ErrorMessage ="Email No valido")]
        public string Email { get; set; }


        

        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",ErrorMessage ="Contraseña debe ser correcta")]
        [MinLength(8), MaxLength(100)]
        public string Password { get; set; }
    }
}
