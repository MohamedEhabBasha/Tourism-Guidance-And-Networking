

namespace Tourism_Guidance_And_Networking.Core.Attribute
{
    public class AllowedExtenstionsAttribute : ValidationAttribute
    {
        private readonly string _allowedExtenstions;
        public AllowedExtenstionsAttribute(string allowedExtenstions)
        {
            _allowedExtenstions = allowedExtenstions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                var extension = Path.GetExtension(file.FileName);

                var isAllowed = _allowedExtenstions.Split(',').Contains(extension, StringComparer.OrdinalIgnoreCase);

                if (isAllowed)
                {
                    return ValidationResult.Success;
                }
                else
                    return new ValidationResult($"Only {_allowedExtenstions} is Allowed");
            }
            return new ValidationResult("Not Valid File");
        }
    }
}
