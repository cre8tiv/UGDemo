#!/bin/bash
cp /tmp/backup/DemoDb.bak /var/opt/mssql/data/DemoDb.bak

/opt/mssql-tools/bin/sqlcmd \
-S 127.0.0.1 -U SA -P HereIsThePassword! \
-i /tmp/backup/restore.sql