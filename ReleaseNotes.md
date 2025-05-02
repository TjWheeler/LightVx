# LightVx - Release Notes

03/05/2025 - Version 4.0.2

20/04/2023 - Version 4.0.1
Added Image Signature Validators for Jpg, Png and Gif.
.Net Framework Support changed to .Net 4.8 (may break for .net 4 and 4.5)

10/02/2023 - Version 4.0.0
* Attribute Validators for Class Properties.
* Call Validator.Validate(myClassInstance)
Breaking changes in Version 4
* The IsValid method on ValidatorFluent is no longer nullable.
* ValidatorBase.Validate is now public
* Eval is no longer an Extension method on object, you must call Validator.Eval instead.

22/01/2023 - Version 3.1.0
* Added ability to create Validation Defintions (Sets of Validators) to allow a more centralised location for validators to
	be defined then used later as needed.

21/01/2023 - Version 3.0.3
* Added RegExValidator and MatchesExpression to the FluentApi to validated against Regular Expressions.

21/01/2023 - Version 3.0.2
* Updated IsIn validator to allow arrays and collections as inputs.  Validates each item in the input exists in the collection.

24/07/2022 - Version 3.0.1
* The Validator.FieldDisplayName will now default to Validator.FieldName when not set.

24/07/2022 - Version 3.0.0 (Breaking Changes - Only if you are implementing IValidator on your own validators)
* Interface IValidator now specifies FieldDisplayName:string property.
* Update to Length validator to fix output message when no max length set.
* Added Display Name option to allow for both Field and User Friendly names to be used.

23/07/2022 - Version 2.0.8
* Fix to field name's being lost on Fluent API

13/07/2022 - Version 2.0.7
* Fix to Length Validator bug for Array Max length.
* Allowed existing ErrorMessages in ObjectValidator to support overrides.
* Added unit test

19/06/2022 - Version 2.0.6
* Added Apply method to Fluent Validator allowing for a more convenient access style.

15/04/2022 - Version 2.0.5
* Bug fix - email validation should support upper and lower case.  

15/04/2022 - Version 2.0.4
* Changed email validation Regex to support updated standard.

16/11/2020 - Version 2.0.3
* Add fix for allowing apostrophie in Email Address validation.

28/05/2020 - Version 2.0.2
* Added Guid validator
* Updated error label in Int validator to be more descriptive

22/05/2020 - Version 2.0.1
* Added AlphaNumericHyphen validator

20/04/2020 - Version 2.0.0
* ObjectValidator feature allows for construction of an Advanced Validator to validate an entire object.
* Empty, NotEmpty and Length validators now work with Arrays.

04/05/2018 - Version 1.2.0
* Added ContainsValidator - String contains
* Added NotContainsValidator - String not contains
* Added DoesNotTraverse, Contains and DoesNotContain to the fluent api

26/04/2018 - Version 1.1.1
* Added SqlSafeDateValidator - Ensures a date value is within valid SQL date range
* Added InCollectionValidator - Checks if the input is within the items of a collection, optionally ignore case.
* Added IsIn to Fluent Api - Calls the InCollectionValidator
* Updated Min and Max date validators to accept nullable datetime datatypes.

08/03/2018 - Version 1.1.0

* Added USStateValidator and Extensions for Fluent API - use namespace LightVx.Validators.US
* Added Min and Max Date Validators
* Added Min/Max/IsAfter and IsBefore date validation to the Fluent API
* Additional unit test coverage and bug fixes 
* AlphaNumeric and Alpha validators now allow empty string.

20/02/2018 - Version 1.0.3

* Added XssSafeTextValidator
* Added SqlSafeTextValidator
* Updated SafeTextValidator to be an aggregate of WebSafeTextValidator and SqlSafeTextValidator
* Added to the FluentAPI - IsSafeForXss, IsSafeForSql

Note: While the WebSafeTextValidator and SqlSafeValidator attempt to detect potential threats, 
this is not intened to be a full security framework and may not detect all attacks.
Ensure you are using other measures to protect your system.

12/02/2018 - Version 1.0.2

* Added FieldName to ValidatorFluent allowing for usage in result.
* Added IsNumeric to the fluent api.
* Added Validators to the flient api.
