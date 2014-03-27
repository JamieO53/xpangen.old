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
        ('', 0, 4, 0, 'SubClass={Schema,Table}'),
        ('', 0, 5, 0, 'Class=Schema'),
        ('', 0, 6, 0, 'Field=Name'),
        ('', 0, 7, 0, 'Class=Table'),
        ('', 0, 8, 0, 'Field={Name,Schema}'),
        ('', 0, 9, 0, 'SubClass={Column,Index,ForeignKey}'),
        ('', 0, 10, 0, 'Class=Column'),
        ('', 0, 11, 0, 'Field={Name,NativeDataType,ODBCDataType,Length,Precison,Scale,IsNullable,IsKey}'),
        ('', 0, 12, 0, 'SubClass=Default'),
        ('', 0, 13, 0, 'Class=Default'),
        ('', 0, 14, 0, 'Field={Name,Value}'),
        ('', 0, 15, 0, 'Class=Index'),
        ('', 0, 16, 0, 'Field={Name,IsPrimaryKey,IsUnique,IsClusterKey}'),
        ('', 0, 17, 0, 'SubClass={KeyColumn,DataColumn}'),
        ('', 0, 18, 0, 'Class=KeyColumn'),
        ('', 0, 19, 0, 'Field={Name,Order}'),
        ('', 0, 20, 0, 'Class=DataColumn'),
        ('', 0, 21, 0, 'Field=Name'),
        ('', 0, 22, 0, 'Class=ForeignKey'),
        ('', 0, 23, 0, 'Field={Name,ReferenceTable,DeleteAction,UpdateAction}'),
        ('', 0, 24, 0, 'SubClass=ForeignKeyColumn'),
        ('', 0, 25, 0, 'Class=ForeignKeyColumn'),
        ('', 0, 26, 0, 'Field={Name,RelatedColumn}'),
        ('', 0, 27, 0, '.')

-- Database definition
insert	@lines
values	('', 1, 1, 0, 'Database=' + DB_NAME())

-- Schema definition
insert	@lines
select	s.name, 1, 2, 0, 'Schema=' + s.name
from	sys.schemas s
where	exists(
			select	*
			from	sys.tables t
			where	t.schema_id = s.schema_id
			   and	t.name <> 'sysdiagrams'
			)

-- Table definition
insert	@lines
select	s.name + t.name, 2, 1, 0, 'Table=' + t.name + '[Schema=' + s.name + ']'
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
        where	s.name = 'dbo'
		   and	t.name <> 'sysdiagrams'
        ) cTarget
 order  by cTarget.TableName, cTarget.column_id

-- Index definition	
insert	@lines
select	s.name + t.name, 2, 3, x.index_id * 1000,
		'Index=' + x.name +
		'[IsPrimaryKey' + case when x.is_primary_key = 1 then '' else '=''''' end +
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

select  Line
  from	@lines
 order  by Seq, TableName, SubSeq, ColumnSeq
