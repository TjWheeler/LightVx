# Light Vx
LightVx is a light, easy and extensible validation framework for .Net which includes a Fluent API.  
It's intended to help validating user input in apps, or service requests in Web Services or Web API's, or anywhere you need to validate data.

## Breaking Changes in Version 4

The IsValid method on ValidatorFluent is no longer nullable.
ValidatorBase.Validate is now public
Eval is no longer an Extension method on object, you must call Validator.Eval instead.

## Key updates in Version 4
- New Attribute Based Validators
- New File Signature Validators for JPG, PNG, GIF

## Author
Tim Wheeler - https://www.linkedin.com/in/timwheeler/

## Download
[Source](https://github.com/TjWheeler/LightVx) on GitHub 
[Nuget Package](https://www.nuget.org/packages/LightVx) 

## Built-in Validators

* Aggregate - Combines other validators
* AlphaNumeric - Alphabetical or Numbers
* AlphaNumericHyphen - Alphabetical, Numbers, Hyphens and Spaces
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
* SqlSafeText - Detects use of characters used in SQL Injection Attacks
* XssSafeText - Detects use of characters used in XSS Attacks
* SafeText - Combines both the XSS and SQL validators.
* Url - validates against a valid url
* MinDate - Date is equal to or greater than
* MaxDate - Date is equal to or less than
* SqlSafeDateValidator - Checks if a datetime or datetime? is within the valid SQL date range
* InCollectionValidator - Checks if the input is an item within an ICollection.
* ContainsValidator - Checks to ensure the specified content exists within the input
* NotContainsValidator - Checks to ensure the specified content does not exist within the input
* RegExValidator - Validates against a supplied Regular Expression
* JpgSignatureValidator - Validates the image signature magic numbers, accepts a stream, must not be null
* GifSignatureValidator - Validates the image signature magic numbers, accepts a stream, must not be null
* PngSignatureValidator - Validates the image signature magic numbers, accepts a stream, must not be null
* CurrencyValidator - Validates a string is a valid currency format.  Optionally can fallback to a double validation.
* IsoDateValidator - Validate the input is a string, matches the Iso 8601 date format using the date component only.
* IsoTimeValidator - Validate input is matches a Iso 8601 string format for the time component only
* IsoDateTimeValidator - Validate the input is a string, matches the Iso 8601 format and can be converted to a DateTime.
* DateValidator - Validates that a string can be parsed as a DateTime

### File Signature Validators
You can validate a Stream containing an image.  Supports Jpg, Png, Gif.
See: JpgSignatureValidator, PngSignatureValidator, GifSignatureValidator
Note: Stream should be at position 0 before validating.

### Property Attribute Validators - new in Version 4
You can use Attributes on your class properties to define validation requirements.

Example:
```C#
    public class Person
    {
        [Guid]
        public string Id { get; set; }
        [Required, MaxLength(10), NameText]
        public string FirstName { get; set; }
        [Required, MaxLength(15), NameText]
        public string LastName { get; set; }
    }
```

To validate:
```C#
    var person = new Person()
    {
        Id = string.Empty,
        FirstName = "Joe",
        LastName = "Smith"
    };
    var result = Validator.Validate(person);
    Assert.IsTrue(result.IsValid);
    //Get all error messags
    List<string> errorMessages = result.ErrorMessages;
    //Get error messages for each Property
    Dictionary<string, List<string>> fieldErrorMessages = result.FieldErrorMessages;

```

Example 2:
```c#
  public class Product : ModelBase
    {
        [Required, Guid]
        public string Id { get; set; }

        [SafeText, Required, MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [Max(5), NotNull]
        public ImageReference[] Images { get; set; } = Array.Empty<ImageReference>();
        
    }
```


### Object Validation Feature

Recently, this framework has been upgraded to provide a more holistic approach to validating an object.
Object Validation works by creating your own validation class to define all the validators.
The following example validates the Name and Date of Birth properties of the Customer.
```
public class CustomerValidator : ObjectValidator<Customer>
    {
        public CustomerValidator(Customer customer) : base(customer)
        {
            Eval(Input.Name, "Name")
                .IsNotEmpty()
                .HasMinLength(3)
                .HasMaxLength(120)
                .IsNameText(); //Allows alpha, hyphen, space, apostrophie

            Eval(Input.DOB, "Date of Birth")
                .IsNotNull()
                .IsBefore(DateTime.Now);
        }
    }
```
Accessing the validator is similiar to all other validators, but note that the ObjectValidator does not implement `IValidator`.
Instead it implements 'IObjectValidator<T>'.
Here is an example based on calling the above CustomerValidator:
```
	var customer = new Customer();
    customer.Name = "Joe Someone";
    customer.DOB = DateTime.Now.AddYears(-25);

    var validator = new CustomerValidator(customer);
	Assert.IsTrue(validator.IsValid, $"Failed to validate: {validator.ToString()}" );
```

### Location Specific Validators
#### US - United States
Note: To use the US Validators in the Fluent API add use namespace: LightVx.Validators.US

* USStateValidator - Checks values against a known list of US state codes (2 characters uppercase)

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

Example 2
```
    var input = "123ABC";
    if(!Validator.Eval(input, "Customer ID").Required().IsGuid().IsValid) {
        //... validation failed 
    }
```

Example 3
```
    var onFail = new Action<List<string>, List<IValidator>>((list, validators) =>
    {
        foreach (var validator in validators)
        {
            ///...do something
        }
                
    });
    Validator.Eval(item.Name, nameof(item.Name)).IsSafeForXss().IsNotNull().HasMinLength(1).HasMaxLength(150).Fail(onFail).Validate();
           
```
For more examples, see below.

Available Methods

* Required() - must not be null or empty string
* HasLength(int min, int? max)
* IsAlphaNumeric()
* IsAlphaText()
* IsDecimal()
* IsDouble()
* IsCurrency()
* IsEmailAddress()
* IsNumeric()
* IsPhoneNumber()
* IsSafeText()
* IsSafeForSql()
* IsSafeForXss()
* IsUrl()
* Min(int value)
* Min(double value)
* Min(decimal value)
* Max(int/double/decimal/date value)
* IsEmpty()
* IsNotEmpty()
* IsNull()
* IsNotNull()
* HasMinLength(int minLength)
* HasMaxLength(int maxLength)
* IsAfter(DateTime date)
* IsBefore(DateTime date)
* IsAfter(DateTime date)
* IsBetween(DateTime startDate, DateTime endDate)
* IsSqlDate()
* IsIn(ICollection items, bool ignoreCase = false)
* DoesNotTraverse() 
* Contains(string content, bool ignoreCase = false)
* DoesNotContain(string content, bool ignoreCase = false)
* MatchesExpression(string)

US Validation Extensions

* IsUSState() - Checks values against a known list of US state codes (2 characters uppercase)

## Fluent API Examples

**Example using Result**
```
    var input = 100; //user input to be validated
    var result = Validator.Eval(input, "Quantity Requested")
        .Required()
        .Min(50)
        .Max(100);
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

**Validation Set using Validation Definitions**
```
    var nameValidationSet = Validator.Define().Required().IsNameText().HasMaxLength(50);
    string firstName = "Joe";
    if(firstName.Eval(firstName, nameValidationSet).Validate().IsValid == false)
    {
        //Value Invalid
    }
```

**MVC.net Controller Example using Validation Definitions**
```
    //The onFail delegate will add a model state error using the Field Name to match it to the appropriate Form control.
    Action<List<string>, List<IValidator>> onFail = (list, validators) => {
        foreach (var validator in validators)
        {
            modelState.AddModelError(validator.FieldName, validator.ErrorMessage);
        }
    };
    var nameValidationSet = Validator.Define().Required().IsNameText().HasMaxLength(50);
    string firstName = "!@#";
    string lastName = "Smith";
    
    //This
    firstName.Eval("FirstName", "First Name", nameValidationSet).Fail(onFail);
    lastName.Eval("LastName", "Last Name", nameValidationSet).Fail(onFail);
    
    //Or
    firstName.Eval("FirstName", "First Name").ValidateWith(nameValidationSet).Fail(onFail);
    lastName.Eval("LastName", "Last Name").ValidateWith(nameValidationSet).Fail(onFail);
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