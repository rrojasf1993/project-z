declare @idFactura uniqueidentifier;
SELECT @idFactura = id from DocumentTypes where Name like '%Factura Supermercado%';
print(@idFactura)

insert into FieldType (Id, Description, Notes,SrcDocumentTypeId, CreatedAt, UpdatedAt)
values (newid(),'Numero Factura', 'Numero Factura', @idFactura, getdate(),getdate());


insert into FieldType (Id, Description, Notes,SrcDocumentTypeId, CreatedAt, UpdatedAt)
values (newid(),'Direccion Tienda', 'Direccion tienda', @idFactura, getdate(), getdate());

insert into FieldType (Id, Description, Notes, SrcDocumentTypeId,CreatedAt, UpdatedAt)
values (newid(),'Fecha Generacion', 'Fecha generacion',@idFactura, getdate(), getdate());


insert into FieldType (Id, Description, Notes, SrcDocumentTypeId, CreatedAt, UpdatedAt)
values (newid(),'Cufe', 'Cufe', @idFactura,getdate(), getdate());


insert into FieldType (Id, Description, Notes, SrcDocumentTypeId, CreatedAt, UpdatedAt)
values (newid(),'Numero Caja', 'Numero Caja', @idFactura,getdate(), getdate());
