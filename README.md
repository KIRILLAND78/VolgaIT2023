# VolgaIT2023
Для запуска требуется база данных postgres.<br>
Клонируется с помощью 
```git clone https://github.com/KIRILLAND78/VolgaIT2023.git```
или скачивается один из релизов (в гите должен быть, если я не забыл загрузить).<br>
Параметры запуска прописываются в appsettings.json:
```
"DATABASE_CONNECTION_STRING": "Server=localhost;Port=5432;Database=postgresdb;Username=postgresUserName;password=password",
"JWT_KEY": "123_JWT_KEY_1234",
"TOKEN_LIFETIME": 60,
```
или в environment variables.<br>
Пример правильно настроенного appsetting.json
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JWT_KEY": "123_JWT_KEY_1234",
  "DATABASE_CONNECTION_STRING": "Server=localhost;Port=5432;Database=postgresdb;Username=postgres;password=pass",
  "TOKEN_LIFETIME": 60,
  "AllowedHosts": "*"
}
```
Запускается либо уже скомпилированная версия через VolgaIT2023.exe или VolgaIT2023.dll (через консоль).<br>
Либо через visual studio.<br>
При запуске должно выйти подобное:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\Kiri\source\repos\VolgaIT2023\bin\Debug\net7.0\publish
```
В данном примере приложение расположено на http://localhost:5000, swagger включен в production и доступен по http://localhost:5000/swagger/index.html.<br>
<br>
Стоит добавить, что по умолчанию asp разрешает в 5-минутном диапазоне использовать токены после истечения срока их жизни. 
