MathLog.db not included, must be made before building MathLog library
```SQL
CREATE TABLE IF NOT EXISTS MathLog (MathLog_ID INTEGER PRIMARY KEY,
	Message TEXT NOT NULL,
	Time TEXT NOT NULL,
	Function TEXT NOT NULL);
```
