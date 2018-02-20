# LightVx - Release Notes

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