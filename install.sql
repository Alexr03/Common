create table ar_common_configurations
(
    id       int         not null,
    moduleId varchar(36) not null,
    name     text        not null,
    typeName text        null,
    contents text        not null,
    app_data text        not null,
    primary key (id, moduleId)
);

create table ar_common_sql_scripts
(
    id       int         not null,
    moduleId varchar(36) not null,
    name     text        null,
    contents text        null,
    app_data text        null,
    primary key (id, moduleId)
);

