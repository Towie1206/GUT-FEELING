# MASTER BLUEPRINT TUẦN 14 - 20 (HỒI 2): BIẾN CỐ MẤT ĐIỆN, HỘP CẦU DAO, TIVI CAMERA GIẤU KÍN, MINIGAME 2D PC `RoomEscape.exe`, KÉT SẮT TOILET & SẠC IPHONE CŨ

> **Mục tiêu Tuần 14 - 20 (Hồi 2 - "Thực Tại Sứt Mẻ & Âm Mưu Gaslighting"):**  
> Chuyển hóa bầu không khí từ sinh hoạt đời thường ấm cúng của Hồi 1 sang nỗi sợ tâm lý ngột ngạt (Psychological Horror) dựa trên **100% yếu tố con người thực tế**: Kẻ ký sinh sống lén sau bức tường (Squatter — cựu lập trình viên tâm thần thông đồng với chủ nhà) bắt đầu thao túng Nam bằng hệ thống điện, camera quay lén giấu sau tranh và trò chơi 2D `RoomEscape.exe` nhằm ép Nam tìm ra và mở chiếc két sắt âm tường sau gương Toilet.

---

## TỔNG QUAN DÂY CHUYỀN CỜ CỐT TRUYỆN HỒI 2 (STORY FLAGS & PUZZLE ARCHITECTURE)

```text
[Cuối Hồi 1: StoryFlag.SeenStrangerInRain]
       │
       ├──► 1. Sập nguồn điện toàn căn hộ (PowerOutageManager - Tuần 14)
       │         ├── Sấm sét nổ + CinemachineImpulseSource rung lắc màn hình
       │         ├── Đèn tắt phụt, cốc nước trên bàn bị hất đổ lênh láng
       │         └──► Bật cờ: StoryFlag.PowerOutageOccurred
       │
       ├──► 2. Mở ngăn kéo nhặt Đèn Pin (FlashlightItem) & Kiểm tra Hộp Cầu Dao (FuseBoxPuzzle - Tuần 14-15)
       │         ├── Mở nắp hộp cầu dao ở phòng kho -> Bật cờ: StoryFlag.FuseBoxOpened
       │         ├── Tìm Cầu chì Đỏ (dưới gầm giường) -> Bật cờ: StoryFlag.RedFuseFound
       │         ├── Tìm Cầu chì Vàng (hộc tủ bếp) -> Bật cờ: StoryFlag.YellowFuseFound
       │         └── Gắn 2 cầu chì vào ItemSocket -> Gạt cần dao -> Bật cờ: StoryFlag.PowerRestored
       │
       ├──► 3. Sự leo thang tâm lý (Gaslighting Escalation - Tuần 16):
       │         ├── [Bát mì trên bàn]: Tự chuyển từ HalfEaten -> EmptyByStalker + Xuất hiện cốc nước lạ uống dở!
       │         │     └──► Bật cờ: StoryFlag.HalfEatenNoodleSeen
       │         └── [Tivi CRT phòng khách (TVController)]: Tự động bật kênh RenderTexture đen trắng
       │               quay lén chính lưng của Nam từ góc cao sau bức tranh treo tường!
       │               └──► Bật cờ: StoryFlag.CreepyTVBroadcastSeen
       │
       ├──► 4. Máy tính PC & Minigame 2D Meta-Horror "RoomEscape.exe" (RoomEscapeMinigame - Tuần 17-18):
       │         ├── Cinemachine Virtual Camera blend từ mắt Nam (3D FPS) vào màn hình CRT (2D Canvas)
       │         ├── Điều khiển nhân vật Pixel đi vào phòng tắm 2D -> Đẩy tấm gương 2D làm lộ Két sắt
       │         └──► Bật cờ: StoryFlag.RoomEscapeMinigameCompleted & StoryFlag.BathroomSafeRevealed
       │
       ├──► 5. Đẩy gương thật trong Toilet & Mở Két Sắt Âm Tường (BathroomMirrorSafe - Tuần 18):
       │         ├── Trượt tấm gương 3D sang bên -> Mở cửa két sắt
       │         ├── Thu thập: Chìa khóa "Tủ áo", CMND bị gạch mặt (có ngày sinh), iPhone cũ cạn pin
       │         └──► Bật cờ: StoryFlag.BathroomSafeOpened
       │
       └──► 6. Sạc Pin iPhone cũ & Giải mật khẩu 6 số (PhoneChargingPuzzle - Tuần 19-20):
                 ├── Đặt iPhone vào ổ cắm sạc (ItemSocket: "OldPhone") -> Bật cờ: StoryFlag.PhonePluggedIn
                 ├── Soi 3D (Inspect) tấm CMND để lấy ngày sinh "140898" nhập vào màn hình khóa
                 ├── Mở khóa đọc tin nhắn nháp chưa kịp gửi: "Có ai đó đang sống trong tủ quần áo của tôi..."
                 └──► Bật cờ: StoryFlag.PhoneUnlocked & StoryFlag.LastTenantMessageRead (Bước vào Hồi 3!)
```

---

## MODULE 1: KIẾN TRÚC KHUNG GIẢI ĐỐ CHUẨN SOLID (`IPuzzle` & `BasePuzzle`)

Để đảm bảo nguyên lý **Open/Closed (OCP)** và **Liskov Substitution (LSP)**, mọi câu đố trong Hồi 2 (`FuseBoxPuzzle`, `RoomEscapeMinigame`, `BathroomMirrorSafe`, `PhoneChargingPuzzle`) đều kế thừa từ lớp trừu tượng [`BasePuzzle.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/BasePuzzle.cs) và thực thi hợp đồng [`IPuzzle.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/IPuzzle.cs).

### 1.1. Sơ đồ Kế thừa (Class Hierarchy)
```text
                  «interface»
                    IPuzzle
                       ▲
                       │
                 «abstract»
                 BasePuzzle  (Template Method: CompletePuzzle() tự bắn StoryFlag + Dialogue)
                       ▲
       ┌───────────────┼───────────────┬────────────────────┐
       │               │               │                    │
 FuseBoxPuzzle  RoomEscapeMinigame  BathroomMirrorSafe  PhoneChargingPuzzle
```

### 1.2. Tại sao dùng Template Method ở `BasePuzzle`?
- Trong [`BasePuzzle.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/BasePuzzle.cs), hàm `protected virtual void CompletePuzzle()` đóng gói sẵn toàn bộ quy trình hậu kỳ khi giải đố thành công:
  1. Chặn kích hoạt lặp (`if (IsSolved) return;`).
  2. Bật `IsSolved = true` và phát sự kiện `OnPuzzleSolved?.Invoke(this)`.
  3. Tự động đẩy cờ cốt truyện (`flagToTriggerOnSolve`) lên `StoryEventBus.Instance`.
  4. Tự động bắn câu độc thoại (`solvedDialogue`) của Nam.
- Nhờ vậy, các lớp con chỉ cần tập trung 100% vào logic đặc thù của câu đố đó trong hàm `EvaluatePuzzle()` (Đúng tinh thần **Single Responsibility Principle - SRP**).

---

## MODULE 2: BIẾN CỐ MẤT ĐIỆN & CÂU ĐỐ HỘP CẦU DAO (TUẦN 14 - 15)

### 2.1. Bộ Điều Khiển Sập Nguồn (`PowerOutageManager.cs`)
File Skeleton đã tạo tại: [`Assets/_Project/Scripts/Environment/PowerOutageManager.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/PowerOutageManager.cs)

**Cách Setup trong Unity Editor:**
1. Tạo một Empty GameObject tên là `PowerOutageManager` trong Scene.
2. Kéo toàn bộ các bóng đèn chiếu sáng trong căn hộ (`Point Light`, `Spot Light` trần nhà) vào mảng `Apartment Lights`.
3. Kéo đối tượng `ThunderEffect` vào ô `Thunder Effect`.
4. Đặt một chiếc cốc thủy tinh (`Rigidbody`, ban đầu tích chọn `Is Kinematic = true`) ở mép bàn ăn và kéo vào ô `Glass Cup To Knock Over`.
5. Ngay sau khi Cutscene Ban công kết thúc (`StoryFlag.SeenStrangerInRain`), `PowerOutageManager` sẽ tự đếm ngược `4.0s`, giáng một tiếng sấm cực đại làm rung màn hình, tắt phụt toàn bộ đèn và hất văng chiếc cốc xuống sàn!

### 2.2. Hai Chiếc Cầu Chì (`FuseItem.cs`) & Hộp Cầu Dao (`FuseBoxPuzzle.cs`)
File Skeleton đã tạo tại:
- [`Assets/_Project/Scripts/Puzzles/FuseItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/FuseItem.cs)
- [`Assets/_Project/Scripts/Puzzles/FuseBoxPuzzle.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/FuseBoxPuzzle.cs)

**Cách Setup Hộp Cầu Dao với `ItemSocket` có sẵn của bạn:**
1. Trên model Hộp Cầu Dao trong phòng kho, tạo 2 GameObject con làm khe cắm và gắn script [`ItemSocket.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/ItemSocket.cs):
   - Khe 1 (`RedFuseSocket`): Đặt `Required Item Tag = "RedFuse"`.
   - Khe 2 (`YellowFuseSocket`): Đặt `Required Item Tag = "YellowFuse"`.
2. Tạo 2 Prefab Cầu chì gắn script [`FuseItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/FuseItem.cs):
   - **Cầu chì Đỏ** (giấu dưới gầm giường phòng ngủ): Đặt `Placeable ID = "RedFuse"`, `Pickup Flag = RedFuseFound`.
   - **Cầu chì Vàng** (giấu trong hộc tủ bếp): Đặt `Placeable ID = "YellowFuse"`, `Pickup Flag = YellowFuseFound`.
3. Trên vỏ Hộp Cầu Dao, gắn [`FuseBoxPuzzle.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/FuseBoxPuzzle.cs), kéo `RedFuseSocket` và `YellowFuseSocket` vào Inspector, và chọn `Flag To Trigger On Solve = PowerRestored`.
4. **Mẹo Thiết Kế (Event-Driven):** Bạn cũng có thể nối sự kiện `UnityEvent OnItemPlaced` của cả 2 `ItemSocket` vào hàm `FuseBoxPuzzle.EvaluatePuzzle()` nếu muốn vừa cắm đủ 2 cầu chì là tự động kiểm tra, hoặc để người chơi tự bấm `[E]` gạt cần dao!

---

## MODULE 3: TIVI CRT PHÁT CAMERA GIẤU KÍN & BÁT MÌ BỊ ĂN VỤNG (TUẦN 16)

Khoảnh khắc người chơi gạt cầu dao bật sáng đèn (`StoryFlag.PowerRestored`) là lúc kịch bản tung ra **2 cú đấm tâm lý liên tiếp** mà không cần bất kỳ con ma hay jumpscare rẻ tiền nào:

### 3.1. Bát Mì Bị Ăn Sạch Một Nửa Còn Lại (`NoodleBowlController.cs`)
- Trong [`NoodleBowlController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/NoodleBowlController.cs), khi hàm `HandleStoryFlagChanged(StoryFlag.PowerRestored)` nổ:
  - Bát mì đang từ trạng thái `HalfEaten` (do Nam ăn dở ở Hồi 1) lập tức đổi sang mesh `EmptyByStalker` (cạn sạch nước, đôi đũa bị đặt lệch).
  - Đồng thời bật `mysteriousHalfDrunkCup.SetActive(true)` — một cốc nước lọc mới tinh đã uống dở một nửa nằm ngay cạnh bát mì!
  - Ý nghĩa: Trong lúc mất điện và Nam đang lúi húi tìm cầu chì ở phòng ngủ/kho, **kẻ sống trong tường đã chui ra ngay phòng khách, ăn nốt bát mì của Nam và uống nước!**

### 3.2. Hướng dẫn Setup Camera Quay Lén Sau Bức Tranh lên Tivi CRT (`TVController.cs`)
File Skeleton đã tạo tại: [`Assets/_Project/Scripts/Environment/TVController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/TVController.cs)

**5 bước tạo hiệu ứng Camera giấu kín bằng `RenderTexture` trong Unity Editor:**
1. **Tạo RenderTexture:**
   - Chuột phải trong Project (`Assets/_Project/Materials/`) $\rightarrow$ `Create -> Rendering -> Render Texture`, đặt tên là `RT_HiddenWallCam`.
   - Chỉnh `Size = 512 x 384` (độ phân giải thấp kiểu camera giám sát cũ) và `Filter Mode = Point` để tạo độ gai hạt sắc lạnh.
2. **Tạo Material cho màn hình Tivi Hồi 2:**
   - Tạo Material `M_TV_HiddenCam` (Shader: `Universal Render Pipeline/Unlit` hoặc `Lit` có bật `Emission`).
   - Kéo `RT_HiddenWallCam` vào ô `Base Map` và `Emission Map` (chỉnh màu Emission hơi ám xanh lá hoặc trắng đen u ám).
   - Gán `M_TV_HiddenCam` vào trường `Hidden Camera Mat` của [`TVController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/TVController.cs).
3. **Đặt Camera phụ giấu sau bức tranh (`HiddenWallCamera`):**
   - Trên bức tranh treo tường phía sau lưng sofa (nhìn thẳng về phía Tivi và bàn ăn), tạo một Camera con tên là `HiddenWallCamera`.
   - Xóa `AudioListener` trên `HiddenWallCamera` (để không xung đột với Main Camera).
   - Kéo `RT_HiddenWallCam` vào ô `Output Texture` (`Target Texture`) của `HiddenWallCamera`.
   - **Tối ưu GPU cực kỳ quan trọng:** Mặc định tắt component `HiddenWallCamera.enabled = false` ở Hồi 1! Chỉ khi `StoryFlag.PowerRestored` bật ở Hồi 2, `TVController` mới bật `hiddenWallCamera.enabled = true`.
4. **Đổi Layer hiển thị của Player:**
   - Đảm bảo mesh cơ thể/đầu của Nam được `HiddenWallCamera` nhìn thấy để khi người chơi đứng trước Tivi sẽ thấy **chính bóng lưng của mình đang bị quay lén trực tiếp từ bức tranh phía sau!**

---

## MODULE 4: MINIGAME 2D PIXEL `RoomEscape.exe` TRÊN MÁY TÍNH CRT (TUẦN 17 - 18)

File Skeleton đã tạo tại: [`Assets/_Project/Scripts/Puzzles/RoomEscapeMinigame.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/RoomEscapeMinigame.cs)

### 4.1. Tại sao dùng `CinemachineCamera` + `RenderTexture` cho Máy Tính PC?
Theo chuẩn kiến trúc của dự án (`GEMINI.md`), khi Nam bấm `[E]` ngồi vào bàn máy tính:
1. Ta **KHÔNG** bật một Canvas 2D đè thô thiển toàn màn hình ngay lập tức.
2. Thay vào đó, ta đặt một `CinemachineCamera` (`VCam_PCMonitor`, mặc định `Priority = 0`) căn góc nhìn trực diện vào màn hình lồi của chiếc máy tính CRT trên bàn.
3. Khi `LaunchMinigame()` chạy:
   - Chuyển Player sang `PlayerCutsceneState` (khóa bước chân và xoay chuột 3D).
   - Nâng `pcMonitorVCam.Priority = 20` $\rightarrow$ `CinemachineBrain` tự động zoom mượt góc nhìn của Nam cúi sát vào màn hình máy tính CRT trong `0.8s`!
   - Giao diện 2D Pixel `RoomEscape.exe` có thể đặt trên một **World Space Canvas** gắn sát mặt kính màn hình CRT (hoặc render qua một Camera 2D phụ ra `RenderTexture` dán lên mặt kính cong của màn hình CRT để hưởng trọn hiệu ứng bóng kính và VHS!).

### 4.2. Cấu trúc Hierarchy của Minigame 2D `RoomEscape.exe`
```text
PC_Desk_Setup
 ├── CRT_Monitor_Mesh (Gắn Collider + RoomEscapeMinigame.cs)
 ├── VCam_PCMonitor (CinemachineCamera nhìn thẳng vào màn hình, Priority = 0)
 └── Canvas_RoomEscape2D (World Space Canvas gắn khít mặt màn hình CRT)
      └── Minigame2DRoot (GameObject bật lên khi chơi)
           ├── BG_PixelApartment (Ảnh pixel mặt bằng căn hộ tối tăm)
           ├── PixelSafeIcon (Hình két sắt nằm dưới gương, mặc định ẩn)
           ├── PixelMirrorTransform (RectTransform tấm gương pixel trong phòng tắm 2D)
           └── PixelPlayerTransform (RectTransform nhân vật pixel điều khiển bằng WASD)
```

---

## MODULE 5: KÉT SẮT ÂM TƯỜNG SAU GƯƠNG TOILET & CÂU ĐỐ SẠC IPHONE CŨ (TUẦN 18 - 20)

### 5.1. Đẩy Gương Thật & Mở Két Sắt Toilet (`BathroomMirrorSafe.cs`)
File Skeleton đã tạo tại: [`Assets/_Project/Scripts/Puzzles/BathroomMirrorSafe.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/BathroomMirrorSafe.cs)
- **Tính liên kết Meta-Horror:** Nếu người chơi vào Toilet bấm vào gương **trước khi** chơi xong minigame PC (`!HasFlag(StoryFlag.BathroomSafeRevealed)`), Nam chỉ nói: *"Chỉ là tấm gương phòng tắm hơi ố mờ..."*.
- Ngay khi vừa đẩy gương trong minigame 2D trên máy tính (`StoryFlag.BathroomSafeRevealed = true`), người chơi bước vào Toilet thực tế bấm `[E]` vào gương $\rightarrow$ Tấm gương trượt ngang sang một bên kêu ken két, để lộ **chiếc két sắt âm tường y hệt trong trò chơi điện tử của tên sát nhân!**
- Khi mở cửa két sắt, bật `SetActive(true)` cho 3 vật phẩm bên trong:
  1. **Chìa khóa Tủ áo** (Gắn `HoldableItemBase` hoặc `InspectableItem`, mở đường sang Hồi 3).
  2. **CMND của người thuê trước** (Gắn [`InspectableItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/InspectableItem.cs) có sẵn của bạn để người chơi bấm chuột xoay 360 độ đọc dòng **Ngày sinh: 14/08/1998** ở mặt trước).
  3. **Chiếc iPhone cũ cạn pin** (Gắn `HoldableItemBase` với `Placeable ID = "OldPhone"`).

### 5.2. Cắm Sạc iPhone & Bàn Phím Mật Khẩu 6 Số (`PhoneChargingPuzzle.cs`)
File Skeleton đã tạo tại: [`Assets/_Project/Scripts/Puzzles/PhoneChargingPuzzle.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Puzzles/PhoneChargingPuzzle.cs)
1. Đặt một ổ cắm/dây sạc trên bàn gắn [`ItemSocket.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/ItemSocket.cs) với `Required Item Tag = "OldPhone"`.
2. Khi người chơi cầm chiếc iPhone cũ từ két sắt ra cắm vào dây sạc (`phoneChargerSocket.HasItem == true`), bấm `[E]` vào bộ sạc sẽ mở `lockscreenCanvasUI` hiển thị bàn phím số từ `0 -> 9`.
3. Trên mỗi Button số `0..9` trong Unity UI, gán sự kiện `OnClick()` gọi hàm `PhoneChargingPuzzle.AppendDigit("1")`, `AppendDigit("4")`, v.v.
4. Khi nhập đủ 6 chữ số `"140898"`, hàm `EvaluatePuzzle()` mở khóa màn hình, hiện bức tin nhắn cầu cứu chưa kịp gửi của nạn nhân trước:
   > *"Cứu tôi với... Đêm nào tôi cũng nghe tiếng thở ngay sau vách gỗ. Có ai đó đang sống trong tủ quần áo của tôi..."*
5. Bật cờ `StoryFlag.PhoneUnlocked` và `StoryFlag.LastTenantMessageRead`, chính thức dẫn người chơi cầm chìa khóa bước tới mở chiếc Tủ Quần Áo ở **Hồi 3**!

---

## MODULE 6: CÂU HỎI PHẢN BIỆN CHUẨN BỊ CHO MILESTONE MOCK INTERVIEW (TUẦN 11 - 20)

Khi bạn hoàn thành việc tự code phần `// TODO:` trong các file Skeleton của Hồi 1 và Hồi 2, hãy sẵn sàng cho buổi **Technical Mock Interview** với 4 câu hỏi trọng tâm sau (tham chiếu [`MOCK_INTERVIEW_CONTEXT_GAME_DEV_TOI.md`](file:///e:/Unity/GUT%20FEELING/docs/MOCK_INTERVIEW_CONTEXT_GAME_DEV_TOI.md)):
1. **DIP & Observer Pattern:** Tại sao `TVController`, `NoodleBowlController` và `PowerOutageManager` đều tự lắng nghe `StoryEventBus.Instance.OnStoryFlagChanged` thay vì để `FuseBoxPuzzle` gọi trực tiếp `tv.TurnOnHiddenCam()` và `noodle.MakeEmpty()`? Nếu gọi trực tiếp thì vi phạm nguyên lý nào trong SOLID?
2. **Memory & GPU Optimization:** Chiếc `HiddenWallCamera` (quay RenderTexture cho Tivi Hồi 2) nếu để `enabled = true` suốt từ đầu Hồi 1 đến hết Hồi 3 sẽ gây lãng phí tài nguyên GPU (Draw Calls, Shadow Pass, Overdraw) như thế nào? Khi nào cần bật và khi nào bắt buộc phải tắt nó?
3. **LSP & Template Method:** Trong `BasePuzzle`, tại sao `CompletePuzzle()` lại được đánh dấu là `protected virtual` còn `EvaluatePuzzle()` là `public abstract`?
4. **Event Leak Prevention:** Điều gì sẽ xảy ra khi chuyển Scene hoặc Destroy object nếu bạn đăng ký `director.stopped += OnDirectorStopped` hoặc `StoryEventBus.Instance.OnStoryFlagChanged += ...` trong `OnEnable()` mà quên hủy đăng ký (`-=`) trong `OnDisable()`?
