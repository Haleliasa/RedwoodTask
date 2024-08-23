## Общая структура
Ассеты имеют структуру пути `Assets/{ModuleName}/*` (примеры: `Assets/Zombies/Zombie.prefab`, `Assets/Player/Sprites/PlayerIdle.png`).

Ассеты, содержащие исходный код, имеют структуру пути `Assets/{ModuleName}/Source/*` (пример: `Assets/Collectables/Source/CollectableView.cs`).

## Персонаж игрока
[`Assets/Player/`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player)

### Персонаж
[`Assets/Player/Source/PlayerCharacter.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerCharacter.cs)

Соединяет независимые части персонажа: движение, стрельба, отображение и т.д.:
* Включает/выключает части персонажа во время игровых событий (старт, конец игры, рестарт);
* Слушает и передает состояния между частями персонажа.

### Движение
[`Assets/Player/Source/PlayerMovement.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerMovement.cs)

Подписывается на `input action` движения, двигает объект персонажа.

### Стрельба
[`Assets/Player/Source/PlayerShoot.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerShoot.cs)

* Хранит число патронов;
* Подписывается на `input action` стрельбы, запускает снаряды ([`Projectile`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Projectiles/Source/Projectile.cs)) в текущем направлении (если есть патроны);
* Сообщает об изменении числа патронов.

Имеет возможность передать управление таймингом стрельбы (моментом вылета снаряда), приняв реализацию [`IShootTiming`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/IShootTiming.cs) (паттерн "Стратегия").
Например, можно реализовать вылет снаряда в определенный кадр анимации.

### Сбор ресурсов
[`Assets/Player/Source/PlayerCollector.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerCollector.cs)

Обрабатывает коллизии с собираемыми предметами ([`ICollectable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Collectables/Source/ICollectable.cs)), сообщает о собранном предмете, вызывает `Collect` (в дефолтной реализации - убрать предмет со сцены).

### Получение урона
[`Assets/Player/Source/PlayerHittable.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerHittable.cs)

Реализует [`IHittable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Core/Source/IHittable.cs): сообщает о смерти при получении любого урона.

### Отображение
[`Assets/Player/Source/PlayerView.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerView.cs)

* Принимает и передает параметры состояния аниматору;
* Разворачивает спрайты персонажа в зависимости от направления движения.

### Анимация
[`Assets/Player/Animations/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Player/Animations)

Анимация разделена на 2 слоя:
1. Базовый, отвечает за состояния бега и бездействия всего тела;
2. Верх, перекрывает верхнюю часть тела в состоянии стрельбы, позволяя стрелять как стоя, так и при беге.

## Зомби
[`Assets/Zombies/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Zombies)

### Армия зомби
[`Assets/Zombies/Source/ZombieArmy.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/ZombieArmy.cs)

Управляет списком зомби:
* Инициализирует случайный тип зомби с заданным временным интервалом;
* Передает цель для движения и атаки;
* Останавливает/удаляет зомби при игровых событиях или смерти зомби.

### Зомби
[`Assets/Zombies/Source/Zombie.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/Zombie.cs)

Соединяет независимые части зомби: движение, статы (уровень здоровья), атака и т.д.:
* Включает/выключает части при включении/выключении зомби (по инициативе [`ZombieArmy`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/ZombieArmy.cs));
* Слушает и передает состояния между частями зомби;
* Создает собираемые патроны ([`Collectable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Collectables/Source/Collectable.cs)) на сцене при смерти.

### Статы
[`Assets/Zombies/Source/ZombieStats.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/ZombieStats.cs)

* Хранит уровень здоровья зомби;
* Реализует [`IHittable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Core/Source/IHittable.cs): уменьшает уровень здоровья при нанесении урона, сообщает о получении урона и смерти.

### Движение
[`Assets/Zombies/Source/ZombieMovement.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/ZombieMovement.cs)

Принимает цель и двигает объект зомби в ее направлении.

### Атака
[`Assets/Zombies/Source/ZombieAttack.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/ZombieAttack.cs)

Обрабатывает коллизии с атакуемыми целями ([`IHittable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Core/Source/IHittable.cs)), наносит смертельный урон.

### Отображение
[`Assets/Zombies/Source/ZombieView.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Source/ZombieView.cs)

* Принимает и передает тип зомби аниматору;
* Разворачивает спрайт зомби в зависимости от направления движения.

### Анимация
[`Assets/Zombies/Animations/`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Zombies/Animations)

Анимация разделена по типам зомби внутри `sub-state machine` `Move`.

## Окружение
[`Assets/Environment/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Environment)

### Генерируемое окружение
[`Assets/Environment/Source/GeneratedEnvironment.cs`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Environment/Source/GeneratedEnvironment.cs)

Генерирует окружение в рантайме по следующим настройкам:
* Настройки генерируемых объектов:
  * Префаб `GeneratedObject` (класс описан ниже);
  * Интервал по оси X в юнитах;
  * Вероятность генерации на очередном интервале;
  * Диапозон Y позиции;
* Список `tiled` спрайт рендереров для растягивания их ширины;

### Генерируемый объект
[`Assets/Environment/Source/GeneratedObject.cs`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Environment/Source/GeneratedObject.cs)

Содержит метод `Generate`, вызываемый `GeneratedEnvironment` при создании объекта. По умолчанию случайно разворачивает спрайты объекта.

### Генерируемое здание
[`Assets/Environment/Source/GeneratedBuilding.cs`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Environment/Source/GeneratedBuilding.cs)

Реализует `Generate`: генерирует здание из блоков по заданным диапозонам ширины и высоты.

### Параллакс
[`Assets/Environment/Source/Parallax.cs`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Environment/Source/Parallax.cs)

## Core

### Event bus
[`Assets/Core/Source/SerializedEvents/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Core/Source/SerializedEvents)

События на основе `ScriptableObject` для связки частей игры (индивидуальные объекты, UI, контроллеры/системы). Примеры:
* Обновление состояния игры при смерти персонажа игрока;
* Обновление UI при изменении числа патронов;
* Обработка кнопок UI контроллером состояния игры.

Контроллер состояния игры на основе `SerializedEvents`: [`Assets/Source/GameController.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Source/GameController.cs)

### Object pool
[`Assets/Core/Source/ObjectPool/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Core/Source/ObjectPool)

Был использован для:
* Зомби;
* Снарядов игрока;
* Собираемых патронов;
* Звуков смерти зомби.

### Injection
[`Assets/Core/Source/Injection/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Core/Source/Injection)

Регистрация зависимостей: [`Assets/Source/SceneContext.cs`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Source/SceneContext.cs)
