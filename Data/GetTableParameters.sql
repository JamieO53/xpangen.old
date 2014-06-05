set nocount on
declare	@objectType table(
	[type]		char(2)	collate Latin1_General_CI_AS_KS_WS	primary key,
	ObjectType	sysname,
	SortOrder	tinyint
	)
insert	@objectType
values	('AF',	'Function', 2),
		('FN',	'Function', 2),
		('IF',	'Function', 2),
		('FS',	'Function', 2),
		('FT',	'Function', 2),
		('TF',	'Function', 2),
		('P',	'Procedure', 3),
		('PC',	'Procedure', 3),
		('X',	'Procedure', 3),
		('U',	'Table', 1),
		('V',	'View', 4)
		
declare @lines table(
    SchemaName	sysname,
    ObjectName	sysname,
    ObjectType	sysname,
    SortOrder	tinyint,
    Seq			int,
    SubSeq		int,
    ColumnSeq	int,
    Line		varchar(280)
    primary key (Seq, SchemaName, ObjectName, ObjectType, SubSeq, ColumnSeq)
    )

-- Definition prefix lines
insert	@lines
values	('', '', '', 0, 0, 1, 0, 'Definition=DatabaseDefinition'),
		('', '', '', 0, 0, 2, 0, 'Class=Database'),
		('', '', '', 0, 0, 3, 0, 'Field=Name'),
		('', '', '', 0, 0, 4, 0, 'SubClass=Schema'),
		('', '', '', 0, 0, 5, 0, 'Class=Schema'),
		('', '', '', 0, 0, 6, 0, 'Field={Name,SchemaName}'),
		('', '', '', 0, 0, 7, 0, 'SubClass={Table,Procedure,Function,View}'),
		('', '', '', 0, 0, 8, 0, 'Class=Table'),
		('', '', '', 0, 0, 9, 0, 'Field={Name,TableName}'),
		('', '', '', 0, 0, 10, 0, 'SubClass={Column,Index,ForeignKey}'),
		('', '', '', 0, 0, 11, 0, 'Class=Column'),
		('', '', '', 0, 0, 12, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey}'),
		('', '', '', 0, 0, 13, 0, 'SubClass=Default'),
		('', '', '', 0, 0, 14, 0, 'Class=Default'),
		('', '', '', 0, 0, 15, 0, 'Field={Name,Value}'),
		('', '', '', 0, 0, 16, 0, 'Class=Index'),
		('', '', '', 0, 0, 17, 0, 'Field={Name,IsPrimaryKey,IsUnique,IsClusterKey}'),
		('', '', '', 0, 0, 18, 0, 'SubClass={KeyColumn,DataColumn}'),
		('', '', '', 0, 0, 19, 0, 'Class=KeyColumn'),
		('', '', '', 0, 0, 20, 0, 'Field={Name,Order}'),
		('', '', '', 0, 0, 21, 0, 'Class=DataColumn'),
		('', '', '', 0, 0, 22, 0, 'Field=Name'),
		('', '', '', 0, 0, 23, 0, 'Class=ForeignKey'),
		('', '', '', 0, 0, 24, 0, 'Field={Name,ReferenceTable,DeleteAction,UpdateAction}'),
		('', '', '', 0, 0, 25, 0, 'SubClass=ForeignKeyColumn'),
		('', '', '', 0, 0, 26, 0, 'Class=ForeignKeyColumn'),
		('', '', '', 0, 0, 27, 0, 'Field={Name,RelatedColumn}'),
		('', '', '', 0, 0, 28, 0, 'Class=Procedure'),
		('', '', '', 0, 0, 29, 0, 'Field={Name,ProcedureName}'),
		('', '', '', 0, 0, 30, 0, 'SubClass=Parameter'),
		('', '', '', 0, 0, 31, 0, 'Class=Parameter'),
		('', '', '', 0, 0, 32, 0, 'Field={Name,ParameterName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,Direction}'),
		('', '', '', 0, 0, 33, 0, 'Class=Function'),
		('', '', '', 0, 0, 34, 0, 'Field={Name,FunctionName}'),
		('', '', '', 0, 0, 35, 0, 'SubClass={Parameter,Column}'),
		('', '', '', 0, 0, 36, 0, 'Class=Parameter'),
		('', '', '', 0, 0, 37, 0, 'Field={Name,ParameterName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,Direction}'),
		('', '', '', 0, 0, 38, 0, 'Class=Column'),
		('', '', '', 0, 0, 39, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey}'),
		('', '', '', 0, 0, 40, 0, 'SubClass=Default'),
		('', '', '', 0, 0, 41, 0, 'Class=Default'),
		('', '', '', 0, 0, 42, 0, 'Field={Name,Value}'),
		('', '', '', 0, 0, 43, 0, 'Class=View'),
		('', '', '', 0, 0, 44, 0, 'Field={Name,ViewName}'),
		('', '', '', 0, 0, 45, 0, 'SubClass=Column'),
		('', '', '', 0, 0, 46, 0, 'Class=Column'),
		('', '', '', 0, 0, 47, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey}'),
		('', '', '', 0, 0, 48, 0, 'SubClass=Default'),
		('', '', '', 0, 0, 49, 0, 'Class=Default'),
		('', '', '', 0, 0, 50, 0, 'Field={Name,Value}'),
		('', '', '', 0, 0, 51, 0, '.')

-- Database definition
insert	@lines
values	('', '', '', 0, 1, 1, 0, 'Database=' + DB_NAME())

-- Schema definition
insert	@lines
select	s.name, '', '', 0, 2, 0, 0, 'Schema=' + replace(s.name,' ', '') + '[SchemaName=''' + s.name + ''']'
from	sys.schemas s
where	exists(
			select	*
			from	sys.objects o
			where	o.schema_id = s.schema_id
			   and	o.name <> 'sysdiagrams'
			)
	and	s.name <> 'sys'

-- Object definition
insert	@lines
select	s.name, o.name,
		z.ObjectType, z.SortOrder, 2, 1, 0,
		z.ObjectType
			+ '='
			+ replace(o.name,' ', '')
			+ '['
			+ z.ObjectType
			+ 'Name='
			+ o.name
			+ ']'
from	sys.schemas s
join	sys.objects o
    on	o.schema_id = s.schema_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	s.name <> 'sys'

-- Column definition
insert	@lines
select  col.SchemaName, col.ObjectName, col.ObjectType, col.SortOrder, 2, 2, col.column_id,
        'Column=' + col.ColumnName +
        '[NativeDataType=' + col.NativeDataType +
        ',ODBCDataType=' + cast(col.ODBCDataType as varchar(10)) +
        ',Length=' + cast(col.Length as varchar(10)) +
        ',Precision=' + cast(col.Precision as varchar(10)) +
        ',Scale=' + cast(col.Scale as varchar(10)) +
        case when col.is_nullable = 1 then ',IsNullable' else '' end +
        case when col.ColumnName is null then ',IsAdded' else '' end +
        ']'
from	(
        select  distinct s.name SchemaName, o.name ObjectName, z.ObjectType, z.SortOrder, c.column_id, c.name ColumnName, y.name NativeDataType,
                y.xtype ODBCDataType, c.max_length Length, c.Precision, c.Scale, c.is_nullable
        from	sys.schemas s
        join	sys.objects o
			on  o.schema_id = s.schema_id
        join	sys.columns c
			on  c.object_id = o.object_id
        join	sys.systypes y
			on  y.xtype = c.system_type_id
		join	@objectType z
			on	z.type = o.type
        where	o.name <> 'sysdiagrams'
			and	y.name <> 'sysname'
			and	s.name <> 'sys'
        ) col
 order  by col.ObjectName, col.column_id

-- Index definition	
insert	@lines
select	s.name, o.name, z.ObjectType, z.SortOrder, 2, 3, x.index_id * 1000,
		'Index=' + x.name +
		'[' + case when x.is_primary_key = 1 then 'IsPrimaryKey' else '' end +
		case when x.is_unique = 1 then ',IsUnique' else '' end +
		case when x.type = 1 then ',IsClustered' else '' end +
		']'
from	sys.schemas s
join	sys.objects o
	on  o.schema_id = s.schema_id
join	sys.indexes x
	on	x.object_id = o.object_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	x.name is not null
	and	s.name <> 'sys'

-- Key Column definition	
insert	@lines
select	s.name, o.name, z.ObjectType, z.SortOrder, 2, 3, x.index_id * 1000 + xc.index_column_id,
		'KeyColumn=' + c.name +
		'[Order=' + case when xc.is_descending_key = 0 then 'Ascending' else 'Descending' end +
		']'
from	sys.schemas s
join	sys.objects o
	on  o.schema_id = s.schema_id
join	sys.indexes x
	on	x.object_id = o.object_id
join	sys.index_columns xc
	on	xc.object_id = o.object_id
	and	xc.index_id = x.index_id
join	sys.columns c
	on	c.object_id = o.object_id
	and	c.column_id = xc.column_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	xc.is_included_column = 0
	and	x.name is not null
	and	s.name <> 'sys'

-- Data Column definition	
insert	@lines
select	s.name, o.name, z.ObjectType, z.SortOrder, 2, 4, x.index_id * 1000 + xc.index_column_id,
		'KeyColumn=' + c.name +
		'[Order=' + case when xc.is_descending_key = 0 then 'Ascending' else 'Descending' end +
		']'
from	sys.schemas s
join	sys.objects o
	on  o.schema_id = s.schema_id
join	sys.indexes x
	on	x.object_id = o.object_id
join	sys.index_columns xc
	on	xc.object_id = o.object_id
	and	xc.index_id = x.index_id
join	sys.columns c
	on	c.object_id = o.object_id
	and	c.column_id = xc.column_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	xc.is_included_column = 1
	and	x.name is not null
	and	s.name <> 'sys'

-- Foreign Key definition	
insert	@lines
select	s.name, o.name + f.name, z.ObjectType, z.SortOrder, 2, 5, 0,
		'ForeignKey=' + f.name +
		'[ReferenceTable=' + [or].name +
		',DeleteAction=' + case f.delete_referential_action when 0 then '''No action''' when 1 then 'Cascade' when 2 then '''Set null''' when 3 then '''Set Default''' end +
		',UpdateAction=' + case f.delete_referential_action when 0 then '''No action''' when 1 then 'Cascade' when 2 then '''Set null''' when 3 then '''Set Default''' end +
		']'
from	sys.schemas s
join	sys.objects o
	on  o.schema_id = s.schema_id
join	sys.foreign_keys f
	on	f.parent_object_id = o.object_id
join	sys.objects [or]
	on	[or].object_id = f.referenced_object_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	s.name <> 'sys'

-- Foreign Key Column definition	
insert	@lines
select	s.name, o.name + f.name, z.ObjectType, z.SortOrder, 2, 5, fc.constraint_column_id,
		'ForeignKeyColumn=' + c.name +
		'[RelatedColumn=' + cr.name +
		']'
from	sys.schemas s
join	sys.objects o
	on  o.schema_id = s.schema_id
join	sys.foreign_keys f
	on	f.parent_object_id = o.object_id
join	sys.foreign_key_columns fc
	on	fc.constraint_object_id = f.object_id
	and	fc.parent_object_id = o.object_id
join	sys.columns c
	on	c.object_id = o.object_id
	and	c.column_id = fc.parent_column_id
join	sys.objects [or]
	on	[or].object_id = f.referenced_object_id
join	sys.columns cr
	on	cr.object_id = [or].object_id
	and	cr.column_id = fc.referenced_column_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	s.name <> 'sys'

update	@lines
	set	Line = replace(Line, '[,', '[')
update	@lines
	set	Line = replace(Line, '[]', '')
update	@lines
	set	Line = replace(Line, 'Length=-1', 'Length=''-1''')

select  Line
  from	@lines
 order  by Seq, SchemaName, SortOrder, ObjectName, SubSeq, ColumnSeq
