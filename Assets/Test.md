Выбор архитектурного подхода

Для игры-пазла стоит придерживаться классического ООП-подхода с элементами Data-Oriented Design (где это критично для производительности). Чистый ECS в данном случае будет overhead’ом из-за его сложности. Переключение между стейтами игры реализовать с помощью паттерна StateMachine. Для работы с UI использовать паттерн MVVM. Узкие места – рендеринг UI (1000 картинок в меню) и поиск соседей при Snap'е.

Общая архитектура приложения

Приложение стоит разделить на 5 слоев. Для связи использовать Service Locator, в будущих итерациях, при расширении функционала, обдумать целесообразность использования DI Container.



Presentation Layer (UI) – MenuScreen, DifficultyPopup, GameplayHUD, VictoryPopup.

Gameplay Layer (Игровой процесс) – GameplayController, PuzzleBoard, PieceTray, PieceEntity.

Application Layer (Жизненный цикл, состояния) – GameStateMachine, SceneLoader, AppBootstrapper.

Data Layer (Модели, конфиги, сохранения) – PuzzleData, GameProgress, DifficultyConfig, PlayerProfile.

Infrastructure Layer (Внешние системы) – AssetProvider, SaveSystem, AddressableLoader, PoolManager.



Архитектура игрового процесса (Gameplay Loop)

Bootstrap → MainMenu → Loading → Gameplay → (Victory / Exit to Menu) → MainMenu



State Machine (состояния приложения)

Каждое состояние — отдельный класс, инкапсулирующий логику инициализации и очистки.

Состояние

Ответственность

BootstrapState

Инициализация DI-контейнера, загрузка конфигов, проверка сохранений

MainMenuState

Активация сцены меню, подписка на события UI (выбор пазла, открытие попапа)

LoadingState

Асинхронная загрузка сцены + картинки пазла через Addressables, отображение прогресса

GameplayState

Активация игрового контроллера, старт отслеживания прогресса, обработка паузы

VictoryState

Блокировка ввода, показ попапа победы, сохранение прогресса, награды.



Схема стейтов и классов

ApplicationStart/AppBootstrapper

GameStateMachine

BootstrapState

DI: IAssetProvider, ISaveSystem, ConfigDB

MainMenuState

MenuScreen + MenuViewModel + OptimizedScrollController

PuzzleCardView + PuzzleCardViewModel

DifficultyPopup + DifficultyPopupViewModel

LoadingState

LoadingScreen + SceneLoader + AddressableAssetProvider

GameplayState

PuzzleBoard

PieceTray (ScrollRect + ObjectPool<TraySlot>)

PieceFactory → PuzzlePiece\[]

DragController (Input)

SnapController + IDropTarget (Board/Tray)

RotationController (IPointerClickHandler)

GroupManager (PieceGroup)

WinChecker

ProgressTracker

ExitAndSave() → SaveSystem

VictoryState

VictoryPopup + SaveSystem.UpdateProgress()

