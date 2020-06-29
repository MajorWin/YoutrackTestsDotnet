# YoutrackTests

В реальности эти тесты должны запускаться в CI, поэтому при запуске nunit-у можно передать параметры:
* `--params=YoutrackAddress=<http://...>` - по умолчанию http://localhost:8080
* `--params=Browser=Firefox|Chrome` - запускает тесты в выбранном браузере. по умолчанию в хроме


# YoutrackApp, запускатор ютрека

Чтобы упростить себе жизнь во время разработки.

Во время разработки jar-ка ютрека лежала в проекте YoutrackApp в папке Resources и при билде копировалась в директорию с исполняемым файлом. В публичный репозиторий выкладывать эту сборку ютрека я не стал.

#### Для запуска ютрека:
* Создать директорию `YoutrackApp/Resources` и положить туда `youtrack-5.2.5-8823.jar`
* Сбилдить YoutrackApp
* Запустить YoutrackApp.exe

ИЛИ

* `YoutrackApp.exe /YoutrackJarPath <path>`

#### Остановить ютрек и удалить директорию с данными ютрека
`YoutrackApp.exe /StopCleanup /YoutrackJarPath <path>`

либо

`YoutrackApp.exe /StopCleanup`, если ютрек лежит по адресу `.\Resources\youtrack-5.2.5-8823.jar`

#### Другие параметры
```
/YoutrackJarPath <path> (.\Resources\youtrack-5.2.5-8823.jar)
/BaseUrl <url> (http://localhost)
/Port <port> (8080)
/HomeDirectory <dir> (.\YoutrackData)
/StopCleanup
/Help
```
