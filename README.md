# Delivery

## Структура проекта

Проект включает:
- API для фильтрации заказов по району и времени.
- Логирование операций с возможностью сохранения в базу данных.
- Валидацию входных данных.

### Файлы для загрузки данных

Для быстрой загрузки данных в систему можно использовать примеры файлов, находящиеся в [examples](examples).

## Функционал

API позволяет:
- Получать заказы для доставки в определённый район города.
- Создавать районы по одиночке и из файла.
- Создавать заказы по одиночке и из файла.

## Запуск проекта

### Запуск на localhost

Запуск должен происходить в ветви [master](../master). Переходим в корневую директорию репозитория и вводим команду, указанную ниже:

```bash
dotnet run --project src/Delivery.Service
```

Для тестировние функционала переходим в сваггер по ссылке http://localhost:5104/swagger/index.html.

#### Конфигурациоя файла `appsettings.json`

### Запуск в Docker
Запуск должен происходить в ветви [master-docker](../master-docker). Переходим в корневую директорию репозитория и вводим команду, указанную ниже:

```bash
docker-compose -f docker-compose.yml up -d
```

Для тестировние функционала переходим в сваггер по ссылке http://localhost:5003/swagger/index.html.

## Разработка и тестирование

### Запуск функциональных тестов

Проект включает несколько тестов для проверки функциональности. Запустите тесты командой:
```bash
dotnet test
```
