set nocount on
declare @selection table(
	SchemaName	sysname,
	ObjectName	sysname,
	RefCount	int
	primary key (ObjectName, SchemaName)
	)

insert	@selection
select	s.name SchemaName, o.name ObjectName, 0
from	sys.objects o
join	sys.schemas s
	on	s.schema_id = o.schema_id
where	s.name like 'IF_%'
--	and	o.type in ('U', 'V', 'P', 'TF', 'FN', 'IF')
	and s.schema_id in (
		select	p.schema_id
		from	sys.procedures p
		where	(	p.name = 'File_Out'
				or	p.name like '%Denormalise'
				)
		)
order	by o.name, s.name

--insert	@selection
--select	s.name SchemaName, o.name ObjectName, 0
--from	sys.objects o
--join	sys.schemas s
--	on	s.schema_id = o.schema_id
--where	s.name in ('dbo', 'Batch')
--	and	o.type = 'U'
--	and o.name not in ('Batch', 'BatchActor', 'BatchExtRef', 'BatchLog', 'BatchState',
--		'old_top_trans', 'results', '_logTable', 'CommandLog', 'PanQuery2', 'dataToBeSent',
--		'Partner_20121031', 'PartnerMap_20121031', 'DispatchItem')
--	and o.name not like 'Arg%'
--	and o.name not like 'ECS\_%' escape('\')
--	and o.name not like 'FDMTBO%'
--	and o.name not like '%Log%'
--	and o.name not like 'Shoprite%'
--	and o.name not like 'temp%'
--	and o.name not like 'tmp%'
--	and o.name not like 'tsu%'
--order	by o.name, s.name

-- Foreign keys
declare @foreignKeyColumn table(
	SchemaName		sysname,
	ObjectName		sysname,
	ForeignKey		sysname,
	ColumnName		sysname,
	ObjectType		sysname,
	SortOrder		tinyint,
	ColumnOrder		tinyint,
    RefCount		int,
	ReferenceSchema	sysname,
	ReferenceObject	sysname,
	ReferenceColumn	sysname,
	DeleteAction	sysname,
	UpdateAction	sysname
	)

--insert @foreignKeyColumn
--values ('dbo', 'Account', 'FK_Account_AccountHost', 'hostID', 'Table', 1, 1, 0, 'dbo', 'Partner', 'partnerID', 'No action', 'No action')
--insert @foreignKeyColumn
--values ('dbo', 'Account', 'FK_Account_AccountOwner', 'ownerID', 'Table', 1, 1, 0, 'dbo', 'Partner', 'partnerID', 'No action', 'No action')

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
    RefCount	int,
    Seq			int,
    SubSeq		int,
    ColumnSeq	int,
    Line		varchar(280)
    primary key (Seq, SchemaName, RefCount, ObjectName, ObjectType, SubSeq, ColumnSeq)
    )

--insert	@foreignKeyColumn
--select	s.name SchemaName, o.name ObjectName,
--		coalesce(k.name, 'FK_' + o.name + '_' + [or].name) ForeignKey,
--		c.name ColumnName,
--		z.ObjectType, z.SortOrder, fc.parent_column_id, 0,
--		sr.name ReferenceSchema, [or].name ReferenceObject, [cr].name ReferenceColumn,
--		'No action' DeleteAction, 'No action' UpdateAction
--from	@selection n
--join	sys.schemas s
--	on	s.name = n.SchemaName
--join	sys.objects o
--	on  o.schema_id = s.schema_id
--    and	o.name = n.ObjectName
--join	sys.columns c
--	on	c.object_id = o.object_id
--cross join	@selection nr
--join	sys.schemas sr
--	on	sr.name = nr.SchemaName
--join	sys.objects [or]
--	on  [or].schema_id = sr.schema_id
--    and	[or].name = nr.ObjectName
--join	sys.columns cr
--	on	cr.object_id = [or].object_id
--join	@objectType z
--	on	z.type = o.type
--left join sys.foreign_keys k
--	on	k.parent_object_id = o.object_id
--	and	k.referenced_object_id = [or].object_id
--left join sys.foreign_key_columns fc
--	on	fc.constraint_object_id = k.object_id
--	and	fc.parent_object_id = o.object_id
--	and	fc.parent_column_id = c.column_id
--	and	fc.referenced_column_id = cr.column_id
--where	n.SchemaName + n.ObjectName <> nr.SchemaName + nr.ObjectName
--	and (	fc.parent_column_id is not null and c.column_id = fc.parent_column_id
--		or	k.parent_object_id is null and c.name = cr.name
--		)
--	and cr.is_identity = 1

insert	@foreignKeyColumn
select	s.name SchemaName, o.name ObjectName, fk.name ForeignKey,
		c.name ColumnName, z.ObjectType ObjectType, z.SortOrder SortOrder,
		fkc.parent_column_id, 0 RefCount, sr.name ReferenceSchema, [or].name ReferenceObject,
		[cr].name ReferenceColumn, fk.delete_referential_action_desc DeleteAction, fk.update_referential_action_desc DeleteAction
from	sys.schemas s
join	sys.objects o on o.schema_id = s.schema_id
join	@objectType z
	on	z.type = o.type
join	sys.foreign_keys fk on fk.parent_object_id = o.object_id
join	sys.foreign_key_columns fkc on fkc.parent_object_id = o.object_id
join	sys.columns c on c.object_id = o.object_id and c.column_id = fkc.parent_column_id
join	sys.objects [or] on [or].object_id = fk.referenced_object_id
join	sys.schemas sr on sr.schema_id = [or].schema_id
join	sys.indexes pk on pk.object_id = [or].object_id and pk.is_primary_key = 1
join	sys.index_columns pkc on pkc.index_id = pk.index_id and pkc.object_id = [or].object_id
join	sys.columns cr on cr.object_id = [or].object_id and cr.column_id = pkc.column_id and cr.column_id = fkc.referenced_column_id
where	s.name like 'IF_%'


declare	@rowcount int
--select	s.SchemaName, s.ObjectName, s.RefCount, max(r.RefCount) ReferencedRefCount
--from	@selection s
--join	@foreignKeyColumn k
--	on	k.SchemaName = s.SchemaName
--	and	k.ObjectName = s.ObjectName
--join	@selection r
--	on	r.SchemaName = k.ReferenceSchema
--	and	r.ObjectName = k.ReferenceObject
--where	s.RefCount < r.RefCount
--	or	(	s.RefCount = r.RefCount
--		and	s.ObjectName < r.ObjectName
--		)
--group	by s.SchemaName, s.ObjectName, s.RefCount

set @rowcount = 1--@@rowcount
while	@rowcount > 0
begin
	--select @rowcount [RowCount]
	update	s
		set RefCount = x.RefCount + 1
	from	@selection s
	join	(	select	s.SchemaName, s.ObjectName, max(r.RefCount) RefCount
				from	@selection s
				join	@foreignKeyColumn k
					on	k.SchemaName = s.SchemaName
					and	k.ObjectName = s.ObjectName
				join	@selection r
					on	r.SchemaName = k.ReferenceSchema
					and	r.ObjectName = k.ReferenceObject
				where	s.RefCount < r.RefCount
					or	(	s.RefCount = r.RefCount
						and	s.ObjectName < r.ObjectName
						)
				group	by s.SchemaName, s.ObjectName
				) x
		on	x.SchemaName = s.SchemaName
		and	x.ObjectName = s.ObjectName

	--select	s.SchemaName, s.ObjectName, s.RefCount, max(r.RefCount) ReferencedRefCount
	--from	@selection s
	--join	@foreignKeyColumn k
	--	on	k.SchemaName = s.SchemaName
	--	and	k.ObjectName = s.ObjectName
	--join	@selection r
	--	on	r.SchemaName = k.ReferenceSchema
	--	and	r.ObjectName = k.ReferenceObject
	--where	s.RefCount < r.RefCount
	--	or	(	s.RefCount = r.RefCount
	--		and	s.ObjectName < r.ObjectName
	--		)
	--group	by s.SchemaName, s.ObjectName, s.RefCount
	set @rowcount = @@rowcount
end

-- Definition prefix lines
insert	@lines
values	('', '', '', 0, 0, 0, 1, 0, 'Definition=DatabaseDefinition'),
		('', '', '', 0, 0, 0, 2, 0, 'Class=Database'),
		('', '', '', 0, 0, 0, 3, 0, 'Field=Name'),
		('', '', '', 0, 0, 0, 4, 0, 'SubClass=Schema'),
		('', '', '', 0, 0, 0, 5, 0, 'Class=Schema'),
		('', '', '', 0, 0, 0, 6, 0, 'Field={Name,SchemaName}'),
		('', '', '', 0, 0, 0, 7, 0, 'SubClass=Object'),
		('', '', '', 0, 0, 0, 8, 0, 'Class=Object[Table,View,Procedure,Function]'),
		('', '', '', 0, 0, 0, 9, 0, 'Field=Name'),
		('', '', '', 0, 0, 0, 10, 0, 'SubClass={Column,Index,ForeignKey,Parameter}'),
		('', '', '', 0, 0, 0, 11, 0, 'Class=Table'),
		('', '', '', 0, 0, 0, 12, 0, 'Field=TableName'),
		('', '', '', 0, 0, 0, 13, 0, 'Class=View'),
		('', '', '', 0, 0, 0, 14, 0, 'Field=ViewName'),
		('', '', '', 0, 0, 0, 15, 0, 'Class=Procedure'),
		('', '', '', 0, 0, 0, 16, 0, 'Field=ProcedureName'),
		('', '', '', 0, 0, 0, 17, 0, 'Class=Function'),
		('', '', '', 0, 0, 0, 18, 0, 'Field=FunctionName'),
		('', '', '', 0, 0, 0, 19, 0, 'Class=Column'),
		('', '', '', 0, 0, 0, 20, 0, 'Field={Name,ColumnName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,IsKey,IsIdentity}'),
		('', '', '', 0, 0, 0, 21, 0, 'SubClass=Default'),
		('', '', '', 0, 0, 0, 22, 0, 'Class=Default'),
		('', '', '', 0, 0, 0, 23, 0, 'Field={Name,Value}'),
		('', '', '', 0, 0, 0, 24, 0, 'Class=Index'),
		('', '', '', 0, 0, 0, 25, 0, 'Field={Name,IsPrimaryKey,IsUnique,IsClusterKey}'),
		('', '', '', 0, 0, 0, 26, 0, 'SubClass={KeyColumn,DataColumn}'),
		('', '', '', 0, 0, 0, 27, 0, 'Class=KeyColumn'),
		('', '', '', 0, 0, 0, 28, 0, 'Field={Name,Order}'),
		('', '', '', 0, 0, 0, 29, 0, 'Class=DataColumn'),
		('', '', '', 0, 0, 0, 30, 0, 'Field=Name'),
		('', '', '', 0, 0, 0, 31, 0, 'Class=ForeignKey'),
		('', '', '', 0, 0, 0, 32, 0, 'Field={Name,ReferenceSchema,ReferenceTable,DeleteAction,UpdateAction}'),
		('', '', '', 0, 0, 0, 33, 0, 'SubClass=ForeignKeyColumn'),
		('', '', '', 0, 0, 0, 34, 0, 'Class=ForeignKeyColumn'),
		('', '', '', 0, 0, 0, 35, 0, 'Field={Name,RelatedColumn}'),
		('', '', '', 0, 0, 0, 36, 0, 'Class=Parameter'),
		('', '', '', 0, 0, 0, 37, 0, 'Field={Name,ParameterName,NativeDataType,ODBCDataType,Length,Precision,Scale,IsNullable,Direction}'),
		('', '', '', 0, 0, 0, 38, 0, '.')

-- Database definition
insert	@lines
values	('', '', '', 0, 0, 1, 1, 0, 'Database=' + DB_NAME())

-- Schema definition
insert	@lines
select	s.name, '', '', 0, 0, 2, 0, 0, 'Schema=' + replace(s.name,' ', '') + '[SchemaName=''' + s.name + ''']'
from	sys.schemas s
where	exists (
			select	SchemaName
			from	@selection
			where	SchemaName = s.name
			)

-- Object definition
insert	@lines
select	s.name, o.name,
		z.ObjectType, z.SortOrder, n.RefCount, 2, 1, 0,
		z.ObjectType
			+ '='
			+ replace(o.name,' ', '')
			+ '['
			+ z.ObjectType
			+ 'Name='''
			+ o.name
			+ ''']'
from	@selection n
join	sys.schemas s
	on	s.name = n.SchemaName
join	sys.objects o
    on	o.schema_id = s.schema_id
    and	o.name = n.ObjectName
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	s.name <> 'sys'

-- Column definition
insert	@lines
select  col.SchemaName, col.ObjectName, col.ObjectType, col.SortOrder, col.RefCount, 2, 2, col.column_id,
        'Column=' + replace(col.ColumnName,' ', '') +
        '[ColumnName=''' + col.ColumnName +
        ''',NativeDataType=' + col.NativeDataType +
        ',ODBCDataType=' + cast(col.ODBCDataType as varchar(10)) +
        ',Length=' + cast(col.Length as varchar(10)) +
        ',Precision=' + cast(col.Precision as varchar(10)) +
        ',Scale=' + cast(col.Scale as varchar(10)) +
        case when col.is_nullable = 1 then ',IsNullable' else '' end +
        case when col.IsKey = 1 then ',IsKey' else '' end +
        case when col.is_identity = 1 then ',IsIdentity' else '' end +
        ']'
from	(
        select  distinct s.name SchemaName, o.name ObjectName, z.ObjectType, z.SortOrder, n.RefCount, c.column_id, c.name ColumnName, y.name NativeDataType,
                y.xtype ODBCDataType, c.max_length Length, c.Precision, c.Scale, c.is_nullable, c.is_identity,
                case when xc.column_id is not null then 1 else 0 end IsKey
        from	@selection n
		join	sys.schemas s
			on	s.name = n.SchemaName
        join	sys.objects o
			on  o.schema_id = s.schema_id
			and	o.name = n.ObjectName
        join	sys.columns c
			on  c.object_id = o.object_id
        join	sys.systypes y
			on  y.xtype = c.system_type_id
		join	@objectType z
			on	z.type = o.type
		left join sys.indexes x
			on	x.object_id = o.object_id
		left join sys.index_columns xc
			on	xc.object_id = o.object_id
			and xc.column_id = c.column_id
        where	o.name <> 'sysdiagrams'
			and	y.name <> 'sysname'
			and	s.name <> 'sys'
        ) col
 order  by col.ObjectName, col.column_id

-- Index definition	
insert	@lines
select	s.name, o.name, z.ObjectType, z.SortOrder, n.RefCount, 2, 3, x.index_id * 1000,
		'Index=' + x.name +
		'[' + case when x.is_primary_key = 1 then 'IsPrimaryKey' else '' end +
		case when x.is_unique = 1 then ',IsUnique' else '' end +
		case when x.type = 1 then ',IsClustered' else '' end +
		']'
from	@selection n
join	sys.schemas s
	on	s.name = n.SchemaName
join	sys.objects o
	on  o.schema_id = s.schema_id
    and	o.name = n.ObjectName
join	sys.indexes x
	on	x.object_id = o.object_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'
	and	x.name is not null
	and	s.name <> 'sys'

-- Key Column definition	
insert	@lines
select	s.name, o.name, z.ObjectType, z.SortOrder, n.RefCount, 2, 3, x.index_id * 1000 + xc.index_column_id,
		'KeyColumn=' + c.name +
		'[Order=' + case when xc.is_descending_key = 0 then 'Ascending' else 'Descending' end +
		']'
from	@selection n
join	sys.schemas s
	on	s.name = n.SchemaName
join	sys.objects o
	on  o.schema_id = s.schema_id
    and	o.name = n.ObjectName
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
select	s.name, o.name, z.ObjectType, z.SortOrder, n.RefCount, 2, 4, x.index_id * 1000 + xc.index_column_id,
		'KeyColumn=' + c.name +
		'[Order=' + case when xc.is_descending_key = 0 then 'Ascending' else 'Descending' end +
		']'
from	@selection n
join	sys.schemas s
	on	s.name = n.SchemaName
join	sys.objects o
	on  o.schema_id = s.schema_id
    and	o.name = n.ObjectName
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
select	distinct
		k.SchemaName, k.ObjectName + ' ' + k.ForeignKey, k.ObjectType, k.SortOrder, n.RefCount, 2, 5, 0,
		'ForeignKey=' + replace(k.ForeignKey, ' ', '') +
		'[ReferenceSchema=' + replace(k.ReferenceSchema, ' ', '') +
		',ReferenceTable=' + replace(k.ReferenceObject, ' ', '') +
		',DeleteAction=''' + k.DeleteAction + '''' +
		',UpdateAction=''' + k.UpdateAction + '''' +
		']'
from	@foreignKeyColumn k
join	@selection n
	on	n.SchemaName = k.SchemaName
	and	n.ObjectName = k.ObjectName

-- Foreign Key Column definition	
insert	@lines
select	k.SchemaName, k.ObjectName + ' ' + k.ForeignKey, k.ObjectType, k.SortOrder, n.RefCount, 2, 5, k.ColumnOrder,
		'ForeignKeyColumn=' + replace(k.ColumnName, ' ', '') +
		'[RelatedColumn=' + replace(k.ReferenceColumn, ' ', '') +
		']'
from	@foreignKeyColumn k
join	@selection n
	on	n.SchemaName = k.SchemaName
	and	n.ObjectName = k.ObjectName

-- Parameters
insert @lines
select	n.SchemaName, n.ObjectName, z.ObjectType, z.SortOrder, n.RefCount, 2, 3, m.parameter_id,
		'Parameter=' + replace(m.name, '@', '') +
		'[ParameterName=''' + m.name +
		''',NativeDataType=' + y.name +
		',ODBCDataType=' + cast(y.xtype as varchar(3)) +
        ',Length=' + cast(m.max_length as varchar(10)) +
        ',Precision=' + cast(m.precision as varchar(10)) +
        ',Scale=' + cast(m.scale as varchar(10)) +
        case when m.has_default_value = 1 then ',IsNullable' else '' end +
		',Direction=' + case when m.is_output = 1 then 'InputOutput' else 'Input' end +
		']'
from	@selection n
join	sys.schemas s
	on	s.name = n.SchemaName
join	sys.objects o
	on  o.schema_id = s.schema_id
    and	o.name = n.ObjectName
join	sys.parameters m
	on	m.object_id = o.object_id
join	sys.systypes y
	on  y.xtype = m.system_type_id
join	@objectType z
	on	z.type = o.type
where	o.name <> 'sysdiagrams'

update	@lines
	set	Line = replace(Line, '[,', '[')
update	@lines
	set	Line = replace(Line, '[]', '')
update	@lines
	set	Line = replace(Line, 'Length=-1', 'Length=''-1''')

---- Fixes for Shoprite RetailRecon
--update	@lines
--	set Line = replace(Line, 'AccountHost', 'Partner')
--where	ObjectName = 'Account FK_Account_AccountHost'
--	and	ColumnSeq = 0

--update	@lines
--	set Line = replace(Line, 'AccountOwner', 'Partner')
--where	ObjectName = 'Account FK_Account_AccountOwner'
--	and	ColumnSeq = 0

--update	@lines
--	set Line = replace(Line, ',IsIdentity', '')
--where	ObjectName = 'EcsChangeScriptHistory'
--	and	ColumnSeq = 1

--update	@lines
--	set Line = replace(Line, ',IsIdentity', '')
--where	ObjectName = 'EcsChangeScriptHistory'
--	and	ColumnSeq = 1

--delete	@lines
--where	ObjectName = 'EcsChangeScriptHistory FK_EcsChangeScriptHistory_EcsReleases'

--update	@lines
--	set Line = replace(Line, ',IsIdentity', '')
--where	ObjectName = 'EcsReleases'
--	and	ColumnSeq = 1

--select  *--Line
--  from	@lines
----where	ObjectName like 'BatchProfileMachine%'
--where	Seq = 2 and SubSeq = 1
-- order  by Seq, SchemaName, SortOrder, ObjectName, SubSeq, ColumnSeq

select  Line
  from	@lines
 order  by Seq, SchemaName desc, SortOrder, RefCount, ObjectName, SubSeq, ColumnSeq
