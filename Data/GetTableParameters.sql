set nocount on
declare @lines table(
    TableName sysname,
    Seq int,
    SubSeq int,
    ColumnSeq int,
    Line varchar(280)
    primary key (Seq, TableName, SubSeq, ColumnSeq)
    )

-- Definition prefix lines
insert  @lines
values  ('', 0, 1, 0, 'Definition=DatabaseDefinition'),
		('', 0, 2, 0, 'Class=Database'),
		('', 0, 3, 0, 'Field=Name'),
		('', 0, 4, 0, 'SubClass=Schema'),
		('', 0, 5, 0, 'Class=Schema'),
		('', 0, 6, 0, 'Field={Name,SchemaName}'),
		('', 0, 7, 0, 'SubClass={Table,Procedure,Function,View}'),
		('', 0, 8, 0, 'Class=Table'),
		('', 0, 9, 0, 'Field={Name,TableName}'),
		('', 0, 0, 0, 'SubClass={Column,Index,ForeignKey}'),
		('', 0, 11, 0, 'Class=Column'),
		('', 0, 12, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey}'),
		('', 0, 13, 0, 'SubClass=Default'),
		('', 0, 14, 0, 'Class=Default'),
		('', 0, 15, 0, 'Field={Name,Value}'),
		('', 0, 16, 0, 'Class=Index'),
		('', 0, 17, 0, 'Field={Name,IsPrimaryKey,IsUnique,IsClusterKey}'),
		('', 0, 18, 0, 'SubClass={KeyColumn,DataColumn}'),
		('', 0, 19, 0, 'Class=KeyColumn'),
		('', 0, 20, 0, 'Field={Name,Order}'),
		('', 0, 21, 0, 'Class=DataColumn'),
		('', 0, 22, 0, 'Field=Name'),
		('', 0, 23, 0, 'Class=ForeignKey'),
		('', 0, 24, 0, 'Field={Name,ReferenceTable,DeleteAction,UpdateAction}'),
		('', 0, 25, 0, 'SubClass=ForeignKeyColumn'),
		('', 0, 26, 0, 'Class=ForeignKeyColumn'),
		('', 0, 27, 0, 'Field={Name,RelatedColumn}'),
		('', 0, 28, 0, 'Class=Procedure'),
		('', 0, 29, 0, 'Field={Name,ProcedureName}'),
		('', 0, 30, 0, 'SubClass=Parameter'),
		('', 0, 31, 0, 'Class=Parameter'),
		('', 0, 32, 0, 'Field={Name,ParameterName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,Direction}'),
		('', 0, 33, 0, 'Class=Function'),
		('', 0, 34, 0, 'Field={Name,FunctionName}'),
		('', 0, 35, 0, 'SubClass={Parameter,Column}'),
		('', 0, 36, 0, 'Class=Parameter'),
		('', 0, 37, 0, 'Field={Name,ParameterName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,Direction}'),
		('', 0, 38, 0, 'Class=Column'),
		('', 0, 39, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey}'),
		('', 0, 40, 0, 'SubClass=Default'),
		('', 0, 41, 0, 'Class=Default'),
		('', 0, 42, 0, 'Field={Name,Value}'),
		('', 0, 43, 0, 'Class=View'),
		('', 0, 44, 0, 'Field={Name,ViewName}'),
		('', 0, 45, 0, 'SubClass=Column'),
		('', 0, 46, 0, 'Class=Column'),
		('', 0, 47, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey}'),
		('', 0, 48, 0, 'SubClass=Default'),
		('', 0, 49, 0, 'Class=Default'),
		('', 0, 50, 0, 'Field={Name,Value}'),
		('', 0, 51, 0, '.')

-- Database definition
insert	@lines
values	('', 1, 1, 0, 'Database=' + DB_NAME())

-- Schema definition
insert	@lines
select	s.name, 2, 0, 0, 'Schema=' + replace(s.name,' ', '') + '[SchemaName=''' + s.name + ''']'
from	sys.schemas s
where	exists(
			select	*
			from	sys.tables t
			where	t.schema_id = s.schema_id
			   and	t.name <> 'sysdiagrams'
			)

-- Table definition
insert	@lines
select	s.name + t.name, 2, 1, 0, 'Table=' + replace(t.name,' ', '') + '[TableName=' + t.name + ']'
from	sys.schemas s
join	sys.tables t
    on	t.schema_id = s.schema_id
where	t.name <> 'sysdiagrams'

-- Column definition
insert	@lines
select  cTarget.TableName, 2, 2, cTarget.column_id,
        'Column=' + cTarget.ColumnName +
        '[NativeDataType=' + cTarget.NativeDataType +
        ',ODBCDataType=' + cast(cTarget.ODBCDataType as varchar(10)) +
        ',Length=' + cast(cTarget.Length as varchar(10)) +
        ',Precision=' + cast(cTarget.Precision as varchar(10)) +
        ',Scale=' + cast(cTarget.Scale as varchar(10)) +
        case when cTarget.is_nullable = 1 then ',IsNullable' else '' end +
        case when cTarget.ColumnName is null then ',IsAdded' else '' end +
        ']'
from	(
        select  distinct s.name + t.name TableName, c.column_id, c.name ColumnName, y.name NativeDataType,
                y.xtype ODBCDataType, c.max_length Length, c.Precision, c.Scale, c.is_nullable
        from	sys.schemas s
        join	sys.tables t
			on  t.schema_id = s.schema_id
        join	sys.columns c
			on  c.object_id = t.object_id
        join	sys.systypes y
			on  y.xtype = c.system_type_id
        where	t.name <> 'sysdiagrams'
		   and	y.name <> 'sysname'
        ) cTarget
 order  by cTarget.TableName, cTarget.column_id

-- Index definition	
insert	@lines
select	s.name + t.name, 2, 3, x.index_id * 1000,
		'Index=' + coalesce(x.name, s.name + '_Idx' + cast(x.index_id as varchar(10))) +
		'[' + case when x.is_primary_key = 1 then 'IsPrimaryKey' else '' end +
		case when x.is_unique = 1 then ',IsUnique' else '' end +
		case when x.type = 1 then ',IsClustered' else '' end +
		']'
from	sys.schemas s
join	sys.tables t
	on  t.schema_id = s.schema_id
join	sys.indexes x
	on	x.object_id = t.object_id
where	t.name <> 'sysdiagrams'

-- Key Column definition	
insert	@lines
select	s.name + t.name, 2, 3, x.index_id * 1000 + xc.index_column_id,
		'KeyColumn=' + c.name +
		'[Order=' + case when xc.is_descending_key = 0 then 'Ascending' else 'Descending' end +
		']'
from	sys.schemas s
join	sys.tables t
	on  t.schema_id = s.schema_id
join	sys.indexes x
	on	x.object_id = t.object_id
join	sys.index_columns xc
	on	xc.object_id = t.object_id
   and	xc.index_id = x.index_id
join	sys.columns c
	on	c.object_id = t.object_id
   and	c.column_id = xc.column_id
where	t.name <> 'sysdiagrams'
   and	xc.is_included_column = 0

-- Data Column definition	
insert	@lines
select	s.name + t.name, 2, 4, x.index_id * 1000 + xc.index_column_id,
		'KeyColumn=' + c.name +
		'[Order=' + case when xc.is_descending_key = 0 then 'Ascending' else 'Descending' end +
		']'
from	sys.schemas s
join	sys.tables t
	on  t.schema_id = s.schema_id
join	sys.indexes x
	on	x.object_id = t.object_id
join	sys.index_columns xc
	on	xc.object_id = t.object_id
   and	xc.index_id = x.index_id
join	sys.columns c
	on	c.object_id = t.object_id
   and	c.column_id = xc.column_id
where	t.name <> 'sysdiagrams'
   and	xc.is_included_column = 1

-- Foreign Key definition	
insert	@lines
select	s.name + t.name + f.name, 2, 5, 0,
		'ForeignKey=' + f.name +
		'[ReferenceTable=' + tr.name +
		',DeleteAction=' + case f.delete_referential_action when 0 then '''No action''' when 1 then 'Cascade' when 2 then '''Set null''' when 3 then '''Set Default''' end +
		',UpdateAction=' + case f.delete_referential_action when 0 then '''No action''' when 1 then 'Cascade' when 2 then '''Set null''' when 3 then '''Set Default''' end +
		']'
from	sys.schemas s
join	sys.tables t
	on  t.schema_id = s.schema_id
join	sys.foreign_keys f
	on	f.parent_object_id = t.object_id
join	sys.tables tr
	on	tr.object_id = f.referenced_object_id
where	t.name <> 'sysdiagrams'

-- Foreign Key Column definition	
insert	@lines
select	s.name + t.name + f.name, 2, 5, fc.constraint_column_id,
		'ForeignKeyColumn=' + c.name +
		'[RelatedColumn=' + cr.name +
		']'
from	sys.schemas s
join	sys.tables t
	on  t.schema_id = s.schema_id
join	sys.foreign_keys f
	on	f.parent_object_id = t.object_id
join	sys.foreign_key_columns fc
	on	fc.constraint_object_id = f.object_id
   and	fc.parent_object_id = t.object_id
join	sys.columns c
	on	c.object_id = t.object_id
   and	c.column_id = fc.parent_column_id
join	sys.tables tr
	on	tr.object_id = f.referenced_object_id
join	sys.columns cr
	on	cr.object_id = tr.object_id
   and	cr.column_id = fc.referenced_column_id
where	t.name <> 'sysdiagrams'

update	@lines
	set	Line = replace(Line, '[,', '[')
update	@lines
	set	Line = replace(Line, '[]', '')
update	@lines
	set	Line = replace(Line, 'Length=-1', 'Length=''-1''')

select  Line
  from	@lines
 order  by Seq, TableName, SubSeq, ColumnSeq
