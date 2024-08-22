## Общая структура
Все ассеты имеют путь `Assets/{ModuleName}/*` (примеры: `Assets/Zombies/Zombie.prefab`, `Assets/Player/Sprites/PlayerIdle.png`).

Все ассеты, содержащие исходный код, имеют путь `Assets/{ModuleName}/Source/*` (пример: `Assets/Collectables/Source/CollectableView.cs`).

## Персонаж игрока
`Assets/Player/`

### Персонаж
[`Assets/Player/Source/PlayerCharacter.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerCharacter.cs)

Соединяет независимые части персонажа: движение, стрельба, отображение и т.д.
* Включает/выключает части персонажа во время игровых событий (старт, конец игры, рестарт);
* Слушает и передает состояния между независимыми частями персонажа.

### Движение
[`Assets/Player/Source/PlayerMovement.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerMovement.cs)

Подписывается на `input action` движения, выполняет перемещение объекта персонажа.

### Стрельба
[`Assets/Player/Source/PlayerShoot.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerShoot.cs)

* Хранит число патронов;
* Подписывается на `input action` стрельбы, запускает снаряды ([`Projectile`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Projectiles/Source/Projectile.cs)) в текущем направлении (если есть патроны);
* Вызывает глобальное событие(-я) ([`SerializedEvent`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Core/Source/SerializedEvents/SerializedEvent.cs)) изменения числа патронов.

Имеет возможность передать управление таймингом стрельбы (моментом вылета снаряда), приняв реализацию [`IShootTiming`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/IShootTiming.cs) (паттерн "Стратегия").
Например, можно реализовать вылет снаряда в определенный кадр анимации.

### Сбор ресурсов
[`Assets/Player/Source/PlayerCollector.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerCollector.cs)

Обрабатывает коллизии с собираемыми предметами ([`ICollectable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Collectables/Source/ICollectable.cs)), сообщает о собранном предмете, вызывает `Collect` (в дефолтной реализации - убрать предмет со сцены).

### Получение урона
[`Assets/Player/Source/PlayerHittable.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerHittable.cs)

Реализует [`IHittable`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Core/Source/IHittable.cs): вызывает глобальное событие(-я) ([`SerializedEvent`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Core/Source/SerializedEvents/SerializedEvent.cs)) смерти при получении любого урона.

### Отображение
[`Assets/Player/Source/PlayerView.cs`](https://github.com/Haleliasa/RedwoodTask/blob/master/Assets/Player/Source/PlayerView.cs)

Принимает и передает параметры состояния аниматору, переворачивает спрайты персонажа в зависимости от направления движения.

### Анимация
[`Assets/Player/Animations/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Player/Animations)

Анимация разделена на 2 слоя:
1. Базовый, отвечает за состояния бега и бездействия всего тела;
2. Верх, перекрывает верхнюю часть тела в состоянии стрельбы, позволяя стрелять как стоя, так и при беге.

## Зомби
[`Assets/Zombies/`](https://github.com/Haleliasa/RedwoodTask/tree/master/Assets/Zombies)
