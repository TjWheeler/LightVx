# Light Vx
LightVx is a light,easy and extensible validation framework for .Net that includes a Fluent API.  
It's intended to help validating user input in apps, or service requests in Web Services or Web API's, or anywhere you need to validate data.

## Author
Tim Wheeler - http://blog.timwheeler.io/

## Download
[Source](https://github.com/TjWheeler/LightVx) on GitHub <br/>
[Nuget Package]() <br/>

## Built-in Validators

* Aggregate - Combines other validators
* AlphaNumeric - Alphabetical or Numbers
* AlphaText - a to Z and spaces
* Decimal  - a decimal value
* Email - email address
* Url - Uri/Url
* Empty - will match an empty string
* HexColor - a valid hex color
* IsNull - matches null
* Length - matches string length.  Supply min and max value.
* MaxLength - matches a max string length
* MinLength - matches a min string length
* Max - validates a number is not greater than x.  If input is an `Array` or `ICollection` then it will validate against number of items.
* Min - validates a number is not less than x.  If input is an `Array` or `ICollection` then it will validate against number of items.
* NotEmpty - must not be empty string
* Numeric - numbers only
* PhoneAndLength - combine phone number validator and length validator
* PhoneNumber - attempts to validate a phone number with optional braces
* SafeText - very restrictive validator that allows a to Z, space, hyphen and apostrophe.
* Url - validates against a valid url

## Fluent API
Using the `Validator.Eval` method you can call a number of validators.  
Example
```
            var input = "123ABC";
            Validator.Eval(input, "Customer ID")
                .Required()
                .IsAlphaNumeric()
                .HasLength(6, 6)
                .Success((() =>
                {
                    Console.WriteLine("Validation succeeded");
                }))
                .Fail((errors, validators) =>
                {
                    Console.WriteLine(string.Join(",", errors));
                    // Validation failed, put your failure logic here
                });
```
For more examples, see below.

Available Methods

* Required() - must not be null or empty string
* HasLength(int min, int? max)
* IsAlphaNumeric()
* IsAlphaText()
* IsDecimal()
* IsEmailAddress()
* IsPhoneNumber()
* IsSafeText()
* IsUrl()
* Min(int value)
* Min(double value)
* Min(decimal value)
* Max(int/double/decimal value)
* IsEmpty()
* IsNotEmpty()
* IsNull()
* IsNotNull()
* HasMinLength(int minLength)
* HasMaxLength(int maxLength)



## Fluent API Examples

**Example using Result**
```
            var input = 100; //user input to be validated
            var result = Validator.Eval(input, "Quantity Requested")
                .Required()
                .Min(50)
                .Max(100)
                .Validate();
            if (result.IsValid == false)
            {
                Console.WriteLine(string.Join(";", result.ErrorMessages));
                //... add failure logic here
            }
```
**Inline Example**
```
            var input = "https://github.com/TjWheeler/LightVx"; //user input to be validated
            Validator.Eval(input, "Source Url")
                .Required()
                .IsUrl()
                .Success((() =>
                {
                    Console.WriteLine("Validation succeeded");
                }))
                .Fail((errors, validators) =>
                {
                    Console.WriteLine(string.Join(",", errors));
                    // Validation failed, put your failure logic here
                });
```

## Examples using Validation Helper
```
            //WebApi - Validate that the text is matches Alphabet only
            var input = "ABC";
            string errorMessage;
            if (Validator.IsNotValid<AlphaTextValidator>(input, "First Name" , out errorMessage))
            {
                return BadRequest($"The input is invalid: {errorMessage}");
            }
```

## Examples using Validators directly

Note: Although you can call the validators directly, using the `Validation` helper is more convenient. 
```
            //Validate numberic
            string input = "123ABC";
            IValidator validator = new NumericValidator();
            validator.Validate(input, "My Field Name");
            if (!validator.IsValid)
            {
                Console.WriteLine("Input: " + input + " return error (" + validator.ErrorMessage + ")");
            }
```

## Extending and creating your own validators

In the following example, we are inheriting from an `AggregateValidator`, this allows us to combine validators.

**Creating a Post Code Validator by combining other validators.**
```
//Step 1: Add the custom validator
public class PostCodeValidator : AggregatedValidator
    {
        public PostCodeValidator()
        {
            AddValidator(new LengthValidator(4, 4));
            AddValidator(new NumericValidator());
        }
    }

//Step 2: Add an extension method
public static class PostCodeValidatorExtension
    {
        public static ValidatorFluent IsPostCode(this ValidatorFluent fluentApi)
        {
            fluentApi.AddValidator(new PostCodeValidator());
            return fluentApi;
        }
    }

//Step 3: Call it to validate input

    public void ExampleCustomValidator()
    {
        string input = "...";
        var isValid = Validator.Eval(input, "MyFieldName")
            .Required()
            .IsPostCode()
            .Fail(((errors, validators) =>
            {
                Console.WriteLine("Example failure: " + string.Join(";", errors));
            })).IsValid;
    }
```

**Creating your own validator**

Create a class and inherit `ValidatorBase`.  The only method you need to implement is `Validate`.  There are some base methods that will make it easy to validate using Regular Expressions.  Here's an example of one of the built in validators.

```
    /// <summary>
    ///     Validate Email Addresses
    /// </summary>
    public class EmailValidator : ValidatorBase
    {
        protected override string Expression => @"^([\&\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (SingleMatch((string) _Input))
                Succeed();
            else
                Fail("is not a valid email address.");
        }
    }

```

When using Regular expressions you can also use the `HasMatch` and `MatchCount` base methods.