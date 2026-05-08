declare @idFacturaDocType uniqueidentifier;
SELECT @idFacturaDocType = id from DocumentTypes where Name like '%Factura Supermercado%';
print(@idFacturaDocType)

declare @idCajaFieldType uniqueidentifier;
SELECT @idCajaFieldType = id from FieldType where Description like '%Numero Caja%';
print(@idCajaFieldType)

declare @singleLineRuleScope uniqueidentifier;
select @singleLineRuleScope =id from RuleScope where Name like '%Linea%'
print @singleLineRuleScope

declare @regexRuleType uniqueidentifier;
select @regexRuleType =id from RuleType where Kind like '%Regex%'
print @regexRuleType

insert into FieldRules (Id, ValidationPattern, ConfidenceWeight, IsActive, Priority, DetectionPattern, FieldName,
                        MinConfidence, UseNextLine, DocumentTypeId, FieldTypeId, ScopeId, TypeId)
values (
           newid(),
           '^(?i)(?:Caja: )[0-9]{1,3}',
           1,
           1,
           1,
           '^(?i)(?:Caja: )[0-9]{1,3}',
           'Numero caja',
           0.5,
           0,
           @idFacturaDocType,
           @idCajaFieldType,
           @singleLineRuleScope,
           @regexRuleType
       );