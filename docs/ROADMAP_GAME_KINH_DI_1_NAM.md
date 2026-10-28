# BẢN KẾ HOẠCH TỔNG THỂ & ROADMAP 52 TUẦN PHÁT TRIỂN GAME KINH DỊ CỐT TRUYỆN (ANTHOLOGY SERIES)

> **Mô hình dự án:** Series Tuyển tập Kinh dị Đô thị (Episodic Anthology Horror - Phong cách *Fears to Fathom*, *MiSide*, *Thần Trùng*)  
> **Tên Series đề xuất:** **Urban Dread** (hoặc *Chuyện Nửa Đêm / Dị Sự Đô Thị*)  
> **Tên Chapter 1:** **The Fourth Bag (Căn Hộ Tầng 4 & Cái Bao Tải Thứ Tư)**  
> **Nền tảng mục tiêu:** PC (Steam, Itch.io)  
> **Engine & Công nghệ:** Unity (Unity 6 / 2022 LTS), Universal Render Pipeline (URP), Unity Cinemachine & Timeline (Cutscenes & Camera Blends), New Input System, C#  
> **Thời lượng Chapter 1:** 60 - 90 phút (Giá trị chơi lại cao với 3 Endings hoàn toàn khác biệt)  
> **Nhân vật:** Nam (Nhân vật chính), Nem (Bạn gái), Gã Ký Sinh Trong Tường (Kẻ đột nhập bệnh hoạn sống lén lút giữa các bức tường), Gã chủ trọ gian xảo, Bác bảo vệ chung cư.

---

## MỤC LỤC TỔNG QUAN

1. **PHẦN I: GAME DESIGN DOCUMENT (GDD) CHUẨN XÁC THEO KỊCH BẢN CHƯƠNG 1**
   - 1.1. Core Hook & Định hướng Series Tuyển tập (Anthology Format)
   - 1.2. Kịch bản Chi tiết 3 Hồi & Dây chuyền Cơ chế Gameplay (Gameplay Beat Sheet)
   - 1.3. Chi tiết 3 Nhánh Kết Thúc (Bad, Normal, True/Secret Ending)
   - 1.4. Thiết kế Mặt Bằng 4 Phòng & Các Vị Trí Gieo Mầm Bí Mật (Environment Layout)
   - 1.5. Hồ sơ Nhân vật & Động cơ Phản diện

2. **PHẦN II: KIẾN TRÚC MÃ NGUỒN C# CHUẨN SOLID (BỘ KHUNG KIẾN TRÚC & FORM SKELETON)**
   - 2.1. Bản đồ áp dụng 5 nguyên lý SOLID trong dự án
   - 2.2. Nhóm 1: Hệ thống tương tác (`IInteractable`, `IHoldInteractable`, `IInspectable`, `PlayerInteractionRaycaster`)
   - 2.3. Nhóm 2: Hệ thống cốt truyện & Event Bus (`StoryFlag`, `IStoryEventService`, `StoryEventBus`)
   - 2.4. Nhóm 3: Khung giải đố (`IPuzzle`, `BasePuzzle`, `PhoneChargingPuzzle`, `RoomEscapeMinigame`)
   - 2.5. Nhóm 4: Hệ thống ẩn nấp & nín thở (`IHidingSpot`, `IBreathController`, `HidingController`)
   - 2.6. Nhóm 5: Hệ thống AI kẻ ký sinh (`INoiseListener`, `IStalkerState`, `StalkerController`)
   - 2.7. Nhóm 6: Hệ thống điện ảnh Cutscenes & Rung lắc (`Cinemachine`, `Timeline`, `ICutsceneService`, `CutsceneController`)

3. **PHẦN III: ROADMAP CHI TIẾT 52 TUẦN (TÍCH HỢP CINEMACHINE & PLAYLIST VIDEO TUTORIAL)**
   - Quý 1 (Tuần 1 - 13): Xây dựng khung Greybox, Core Interaction, Cốt truyện Hồi 1 & Sự kiện Mất điện
   - Quý 2 (Tuần 14 - 26): Cầu dao, Minigame PC, Két sắt Toilet, Sạc điện thoại, Đắp Art & Bản DEMO 25 phút
   - Quý 3 (Tuần 27 - 39): Playtest cộng đồng, Hốc tường bí mật, Màn rượt đuổi Hồi 3, 3 Endings, Lồng tiếng, Tối ưu 60 FPS
   - Quý 4 (Tuần 40 - 52): Đăng ký Steam, Trailer, Steam Next Fest, Gửi Key Streamer & Bấm nút Phát hành Chapter 1

4. **PHẦN IV: HƯỚNG DẪN KỸ THUẬT URP LIGHTING, POST-PROCESSING & ÂM THANH 3D**
   - 4.1. Setup ánh sáng căn hộ chung cư cũ
   - 4.2. Setup Volume Profile kinh dị (ACES, Film Grain, Vignette)
   - 4.3. Spatial Audio 3D (Tiếng bước chân, tiếng sột soạt sau vách tường)

5. **PHẦN V: CHIẾN LƯỢC ĐẶT TÊN, PHÁT HÀNH STEAM & MỞ RỘNG CÁC CHAPTER SAU**
   - 5.1. Phân tích Tên Game & Chiến lược Series Anthology
   - 5.2. Mẫu Email Gửi Key Cho Streamer Quốc Tế & Việt Nam
   - 5.3. Định hướng cho Chapter 2 & Chapter 3

---

# PHẦN I: GAME DESIGN DOCUMENT (GDD) CHUẨN XÁC THEO PHONG CÁCH FEARS TO FATHOM

## 1.1. Core Hook & Định hướng Kể chuyện Series Tuyển Tập (F2F Anthology Framing)

### 1.1.1. Khung Dẫn Chuyện Câu Chuyện Có Thật (True Story Framing)
- Tuân thủ công thức kinh điển của **Fears to Fathom** (*Home Alone, Carson House, Woodbury Getaway*):
  - **Mở đầu bằng bức thư tự sự (The Submitter Hook):** Màn hình đen với font chữ typewriter trắng:
    > *"Câu chuyện sau đây được gửi bởi Nam, 20 tuổi...  
    > 'Những gì tôi trải qua vào đêm bão năm đó kỳ dị đến mức lúc tôi khai báo, ngay cả cảnh sát ban đầu cũng nghĩ tôi bị ảo giác hoặc bịa chuyện... Cho đến khi các điều tra viên 113 trực tiếp bước vào căn chung cư số 404, bật chiếc máy tính đó lên và soi đèn pin vào hốc tường bí mật sau tủ áo.'*
  - Âm thanh tiếng gõ phím lạch cạch hoài cổ, nhạc nền synth ambient u uất, đưa người chơi lập tức vào tâm thế: **đây là một hồ sơ vụ án có thật trong đời sống**.

### 1.1.2. Hợp lý hóa Hiện Thực: Âm mưu "Gaslighting" & Chiếc Máy Tính PC
- **Giải quyết triệt để yếu tố "giả tưởng / siêu nhiên":**
  - Trò chơi minigame 2D pixel `RoomEscape.exe` trên máy tính bàn **KHÔNG PHẢI là hiện tượng ma quái hay siêu nhiên**.
  - Nó là một công cụ **thao túng tâm lý (Gaslighting) bệnh hoạn** do chính **Kẻ Ký Sinh Trong Tường (Squatter)** — vốn là một cựu lập trình viên tâm thần sống lén lút nhiều năm — lập trình sẵn trên chiếc PC cũ trong phòng.
  - Kẻ ký sinh phối hợp với gã chủ nhà gian xảo biến căn hộ thành chiếc bẫy: Hắn dùng camera giấu sau tranh ảnh theo dõi con mồi, bật minigame mô phỏng căn phòng để "vờn", khủng bố tinh thần và lừa nạn nhân tự tay mở két sắt sau gương toilet để hắn đoạt tài sản!
  - **Hiệu quả nghệ thuật:** Tạo cú lừa ngoạn mục cho người chơi tưởng là game meta-horror/ma quái, nhưng hồi kết vỡ òa khi sự thật phơi bày: **Kẻ thù 100% là sự độc ác, bệnh hoạn của con người bằng xương bằng thịt!**

### 1.1.3. 6 Cơ Chế Chữ Ký Chuẩn Phong Cách Fears to Fathom (Signature Mechanics)
1. **Nhiệm vụ sinh hoạt đời thường (Mundane To-Do List):** Dòng chữ trắng thanh mảnh góc trên màn hình nhắc nhở các việc vặt: đun nước, pha mì, khóa cửa, nhắn tin.
2. **Khóa then cài cửa & Chốt xích (Deadbolt & Door Chain):** Nhìn vào chốt cửa chính, giữ chuột để cài then xích kim loại lách cách nhằm tạo cảm giác an toàn giả tạo.
3. **Lỗ nhòm Mắt mèo (Peephole Fisheye Camera):** Ghé mắt vào cửa chính, góc nhìn chuyển sang thấu kính mắt cá quan sát hành lang chung cư tối tăm.
4. **Hệ thống Điện thoại & Tin nhắn Nem (SMS Dialogue System):** Chuông báo rung, rút điện thoại chọn câu trả lời (`Option A/B`) với bạn gái.
5. **Zoom Camera (Giữ chuột phải RMB):** Nheo mắt kéo gần tầm nhìn (FOV thu hẹp từ 75 về 45) để soi rõ đồ vật từ xa.
6. **Ẩn nấp & Đo độ ồn/Nín thở (Stealth & Breath/Noise Meter):** Chui vào tủ áo/toilet, nín thở giữ im lặng tuyệt đối khi kẻ địch đi ngang qua.

---

## 1.2. Kịch bản Chi tiết 3 Hồi & Dây Chuyền Gameplay (To-Do List Pacing)

### HỒI 1: BÌNH YÊN KỲ LẠ & SINH HOẠT ĐỜI THƯỜNG (00:00 - 25:00)
* **Bối cảnh:** Nam (sinh viên mới dọn phòng) bước vào căn hộ tầng 4 trong một đêm mưa bão sấm chớp.
* **To-Do List Nhiệm vụ người chơi:**
  1. `[ ] Đặt chìa khóa và dỡ hành lý:` Nhặt chùm chìa khóa phòng đặt lên bàn trà.
  2. `[ ] Khóa then chốt xích cửa chính:` Quay lại cửa chính, gài chốt kim loại chống trộm.
  3. `[ ] Ghé mắt vào lỗ mắt mèo kiểm tra hành lang:` Nhìn thấy hành lang chung cư vắng tanh, đèn neon chập chờn.
  4. `[ ] Pha ly mì tôm ăn tối:`
     - Bấm thử ấm siêu tốc cũ nhưng bị chập cháy đế điện $\rightarrow$ Nam độc thoại phát hiện bếp từ Arasapha và chiếc nồi nhỏ trong bếp $\rightarrow$ Nhặt nồi ra bồn rửa hứng nước $\rightarrow$ Đặt nồi lên bếp từ Arasapha bật công suất 2000W $\rightarrow$ Đợi nước sôi sùng sục $\rightarrow$ Rót vào cốc mì Handy Hảo Hảo $\rightarrow$ Click/giữ chuột ăn từng miếng (Progression Bites).
  5. `[ ] Nhắn tin báo bình an cho Nem:`
     - Điện thoại rung lên: Tin nhắn của Nem hỏi Nam đã dọn đến nơi an toàn chưa.
     - Người chơi chọn câu trả lời trấn an bạn gái rằng mọi thứ đều ổn, căn phòng khá rộng rãi.
  6. `[ ] Bật TV xem bản tin thời sự & Đóng cửa ban công:`
     - Bấm remote: Bản tin cảnh báo bão lớn đổ bộ, khuyến cáo không ra ngoài.
     - Ra ban công kéo cửa kính: Dưới sân chung cư ngập nước, bóng người mặc áo mưa trùm kín đầu đang ngửa cổ nhìn chằm chằm lên phòng Nam. Tia sét chớp lóa $\rightarrow$ Tiếng sấm giáng xuống $\rightarrow$ Bóng người biến mất!

---

### HỒI 2: THỰC TẠI SỨT MẺ & ÂM MƯU GASLIGHTING (25:00 - 60:00)
* **Biến cố kích hoạt:** Đèn toàn bộ căn hộ phụt tắt! Sấm chớp rền vang làm rung chuyển màn hình (`CinemachineImpulseSource`). Cốc nước bị quơ đổ lênh láng ra sàn.
* **To-Do List Nhiệm vụ người chơi:**
  1. `[ ] Bật đèn pin (phím F):` Mở ngăn kéo tủ phòng khách nhặt đèn pin, soi luồng sáng ấm áp xua tan bóng tối.
  2. `[ ] Kiểm tra hộp cầu dao ở phòng kho:`
     - Vào kho, mở nắp tủ điện (Fuse Box): Phát hiện 2 khe cầu chì bị tháo rời. Thấy chiếc bình cứu hỏa mini han gỉ ở góc kho (*Chekhov's Gun*).
  3. `[ ] Lục tìm 2 cầu chì & Đọc các mảnh nhật ký:`
     - Cầu chì đỏ: Dưới gầm giường phòng ngủ (cúi người rọi đèn nhặt).
     - Cầu chì vàng: Trong hộc tủ bát đĩa phòng bếp.
     - **Thu thập nhật ký người thuê trước:** Kể về việc liên tục nghe tiếng thở sột soạt sau vách tường tủ áo, cảm giác đồ đạc bị dịch chuyển mỗi khi ra ngoài.
  4. `[ ] Khôi phục nguồn điện:` Cắm đủ 2 cầu chì, gạt cần dao. Đèn bật sáng trở lại.
* **Sự leo thang tâm lý (The Gaslighting Escalation):**
  1. `[ ] Ăn nốt bát mì tôm:` Nam bước ra phòng khách: **Bát mì trên bàn đã bị ai đó ăn mất một nửa! Và bên cạnh xuất hiện một CỐC NƯỚC MỚI ĐƯỢC RÓT ĐÃ UỐNG DỞ MỘT PHẦN!**
  2. `[ ] Kiểm tra chiếc TV đang tự bật:`
     - TV tự động phát hình ảnh camera trắng đen từ trên cao... **ghi lại chính Nam đang đứng trước TV từ một góc quay giấu sau bức tranh treo tường!**
  3. `[ ] Kiểm tra tiếng chuông lạ từ máy tính phòng ngủ:`
     - Chiếc PC tự khởi động, mở ra minigame pixel 2D: **`RoomEscape.exe`**.
     - Nam ngồi vào máy tính chơi minigame (mô phỏng chính căn hộ này). Trong minigame, nhân vật pixel đẩy tấm gương phòng tắm sang một bên để lộ chiếc két sắt bí mật.
  4. `[ ] Vào phòng tắm kiểm tra tấm gương:`
     - Nam đẩy tấm gương soi sang bên ngoài đời thực: **Chiếc két sắt âm tường thực sự nằm ở đó!**

---

### HỒI 3: VẠCH TRẦN & ĐỐI MẶT (60:00 - 90:00)

#### Giai đoạn 1: Mở két sắt & Sự thật chiếc điện thoại cũ (Phút 60 - 75)
1. **Mở két sắt toilet:** 
   - Bên trong có: Một chùm chìa khóa cũ có khắc chữ *"Tủ áo"*, CMND bị gạch chéo mặt của người thuê trước, và một chiếc điện thoại iPhone cũ đã cạn kiệt pin.
2. **Puzzle sạc pin & Mở khóa điện thoại:**
   - Nam tìm củ sạc và dây cáp trong ngăn kéo bàn làm việc, cắm sạc cho chiếc iPhone cũ trên bàn phòng khách.
   - Nhập mật khẩu 6 số dựa trên ngày sinh trong CMND người thuê trước.
   - Màn hình hiện tin nhắn cũ chưa gửi: *"Có ai đó đang sống trong tủ quần áo của tôi..."*.
3. **Phát hiện hốc tường bí mật (The Discovery):**
   - Nam cầm chìa khóa mở chiếc tủ quần áo gỗ 3 cánh trong phòng ngủ.
   - Gạt đống quần áo sang bên: Tấm ván sau lưng tủ đã bị cưa rời thành lối đi bí mật dẫn vào khoảng trống 1m giữa hai bức tường!
   - **Cảnh tượng kinh hoàng:** Một chiếc đệm rách bốc mùi, đống vỏ mì tôm, màn hình mini nối với camera sau tranh ảnh, và **những chiếc bao tải lớn chứa đầy quần áo, tư trang của các nạn nhân trước**.
4. **Cuộc gọi định mệnh của Nem:**
   - Điện thoại của Nam rung bần bật: Nem gọi từ dưới sảnh tầng 1:
     > *"Nam ơi! Em đang ở dưới sảnh, nhưng bác bảo vệ bảo phòng tầng 4 anh thuê bị niêm phong 1 tháng nay vì người thuê trước bị sát hại chưa tìm thấy xác?! Bác còn bảo rất nhiều người thuê đã mất tích bí ẩn, tay chủ nhà vì tiền cọc mà câu kết giấu nhẹm... Em thấy điềm chẳng lành nên đã gọi 113 báo cảnh sát rồi, anh đang ở đâu đấy?!"*

#### Giai đoạn 2: Cuộc rượt đuổi & Sinh tồn (Phút 75 - 90)
1. Đúng lúc Nem dứt câu: **Tiếng lạch cạch vang lên ngay ở miệng tủ quần áo.** Kẻ ký sinh đã trèo từ thông gió về tổ!
2. Hắn nhìn thấy Nam trong hang ổ của mình, rít lên một tiếng điên dại.
3. Nam đạp đổ chiếc tủ quần áo để chặn đường hắn, lao ra ngoài phòng khách.
4. **Cửa chính đã bị khóa chặt chốt xích từ bên ngoài!**
5. Nam phải vừa lẩn trốn quanh 4 phòng (tắt đèn pin, trốn gầm giường/toilet, nín thở khi hắn đi ngang qua), vừa tìm công cụ để thoát thân hoặc đối đầu!

---

## 1.3. Chi tiết 3 Nhánh Kết Thúc (3 Endings)

```
                                  [HỒI 3: CAO TRÀO RƯỢT ĐUỔI]
                                               |
              +--------------------------------+-------------------------------+
              |                                |                               |
       [Bị bắt / Lộ diện]             [Tìm kìm cắt chốt xích]         [Tìm 3 mảnh đơn khiếu nại]
              |                                |                      [Lấy bình cứu hỏa ở kho]
              v                                v                      [Nem báo công an từ sảnh]
       ENDING 1 (BAD)                   ENDING 2 (NORMAL)             [Xịt & đập bình cứu hỏa]
     "Cái Bao Tải Thứ Tư"             "Tiếng Cửa Trong Mưa"                    |
                                                                               v
                                                                       ENDING 3 (TRUE/SECRET)
                                                                     "Mọi Chuyện Sáng Tỏ"
```

| Tên Ending | Điều Kiện Kích Hoạt | Diễn Biến & Chi Tiết Điện Ảnh | Ý Nghĩa Nghệ Thuật |
| :--- | :--- | :--- | :--- |
| **Ending 1 (Bad):**<br>*"Cái Bao Tải Thứ Tư"* | Nam bị kẻ đó bắt được trong lúc rượt đuổi HOẶC trốn gầm giường mà quên tắt đèn pin / nín thở thất bại. | Kẻ đó đè nghiến Nam xuống sàn. Màn hình tối sầm kèm tiếng giãy giụa nghẹt thở.<br>Màn hình sáng mờ lại: Góc nhìn từ bên trong một chiếc bao bố thô ráp, thấy ánh sáng le lói qua kẽ đan. Tiếng kéo lê bao tải trên sàn gạch sột soạt. Giọng gã đàn ông thì thào phì phò: *"Mày... cũng sẽ nằm ngoan trong bao giống những đứa trước thôi..."*. Tiếng còi xe cảnh sát vang vọng xa xăm ngoài cửa sổ trong tuyệt vọng. | Nỗi sợ tuyệt đối khi trở thành nạn nhân tiếp theo biến mất không dấu vết sau bức tường. |
| **Ending 2 (Normal):**<br>*"Tiếng Cửa Trong Mưa"* | Nam tìm được cây kìm cộng lực (nằm trong ngăn kéo dưới bồn rửa), cắt đứt xích cửa chính và lao ra ngoài hành lang. | Nam tông sầm cửa chạy ra ngoài, va vào Nem và bác bảo vệ đang đi thang bộ lên. Thấy đông người, kẻ sát nhân thụt lùi vào bóng tối, chui vào đường ống thông gió trốn thoát sang các căn hộ bỏ hoang khác.<br>Cảnh sát phong tỏa chung cư, lôi ra từ sau bức tường hàng chục bao tải chứa đồ nạn nhân suốt 5 năm. Nam ngồi trong xe cứu thương run rẩy nắm tay Nem. Nhưng khi ngước nhìn lên ô cửa thông gió tầng 4 dưới cơn mưa... dường như vẫn có một ánh mắt đen ngòm đang nhìn xuống. | Sống sót về thể xác nhưng nỗi ám ảnh tâm lý sẽ đeo bám suốt cuộc đời vì kẻ thủ ác vẫn đang nhởn nhơ ngoài xã hội. |
| **Ending 3 (True/Secret):**<br>*"Mọi Chuyện Sáng Tỏ"* | 1. Tìm đủ 3 mảnh ghép của tờ đơn khiếu nại cũ giấu trong nhà (vạch trần chính chủ nhà câu kết nuốt tiền cọc).<br>2. Chạy vào phòng kho lấy **Bình cứu hỏa mini**.<br>3. Cố thủ trong toilet và phản đòn hạ gục kẻ địch. | **Nem** ở dưới sảnh, ngay khi nghe bác bảo vệ cảnh báo về vụ án mạng tháng trước đã thấy có điềm lạ và linh tính chẳng lành, nên đã **chủ động gọi Cảnh sát 113 ngay từ đầu**!<br>Khi kẻ sát nhân phá tung cửa toilet lao vào, Nam giật chốt xịt thẳng bọt khí cứu hỏa mù mịt vào mặt hắn làm hắn lóa mắt ngạt thở. Không chần chừ, Nam dùng hết sức bình sinh **đập mạnh bình cứu hỏa vào đầu kẻ địch** khiến hắn ngã gục vào bồn tắm, rồi Nam lập tức sập cửa toilet khóa chốt chặt từ bên ngoài!<br>Công an cùng Nem và bác bảo vệ vừa kịp phá cửa chính ập vào, bắt sống tên sát nhân trong toilet và bắt giữ luôn gã chủ nhà gian ác định trốn thoát dưới sảnh.<br>**Cảnh kết:** Trời sáng, bão tan, mưa tạnh, ánh nắng ban mai rọi sáng căn hộ. Nam bước ra ngoài an toàn, ôm chầm lấy Nem trong sự nhẹ nhõm. Toàn bộ tội ác đen tối của căn chung cư được phơi bày ra ánh sáng pháp luật. | Sự cảnh giác thông minh của Nem kết hợp với lòng dũng cảm phản kháng của Nam; mọi tội ác dù đen tối đến đâu cuối cùng cũng phải trả giá trước ánh sáng công lý. |

---

# PHẦN II: KIẾN TRÚC MÃ NGUỒN C# CHUẨN MỰC

## 2.1. Cây Thư Mục Project Unity

```text
Assets/_Project/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs
│   │   ├── SaveSystem.cs
│   │   └── AudioManager.cs
│   ├── Story/
│   │   ├── StoryFlag.cs
│   │   ├── StoryFlagManager.cs
│   │   └── StoryTrigger.cs
│   ├── Interaction/
│   │   ├── IInteractable.cs
│   │   ├── PlayerInteraction.cs
│   │   ├── InspectableObject.cs
│   │   └── InspectManager.cs
│   ├── Puzzles/
│   │   ├── FuseBox.cs
│   │   ├── PhoneChargingPuzzle.cs
│   │   ├── RoomEscapeMinigame.cs
│   │   └── BathroomMirrorSafe.cs
│   ├── Stealth/
│   │   ├── HideAndSeekManager.cs
│   │   └── StalkerAI.cs
│   └── Player/
│       ├── PlayerController.cs
│       ├── PlayerLook.cs
│       └── Flashlight.cs
```

---

---

# PHẦN II: KIẾN TRÚC MÃ NGUỒN C# CHUẨN SOLID (BỘ KHUNG KIẾN TRÚC & FORM SKELETON)

> 💡 **Tuyên ngôn Kiến trúc:**  
> Hệ thống này được thiết kế theo đúng **5 nguyên lý SOLID**, tách biệt hoàn toàn giữa **Hợp đồng (Interfaces/Contracts)**, **Bộ điều khiển (Controllers)** và **Giao diện/Âm thanh (Observers/UI)**.  
> Toàn bộ logic bên dưới được chuẩn bị sẵn dưới dạng **Form / Skeleton (Khung sườn, method signatures, parameters, XML doc comments)** để bạn tự tay lập trình triển khai (implementation) mà không bị phụ thuộc hay gò bó!

---

## 2.1. Bản Đồ Áp Dụng 5 Nguyên Lý SOLID Trong Dự Án

* **S (Single Responsibility - Đơn trách nhiệm):**  
  * `PlayerInteractionRaycaster` chỉ làm đúng 1 việc: Bắn Raycast tìm vật thể và kích hoạt Interface. Không dính 1 dòng code nào về UI chữ hay âm thanh.
  * `InteractionUIObserver` chỉ lắng nghe sự kiện từ Raycaster để hiển thị icon tâm ngắm và câu lệnh prompt.
* **O (Open/Closed - Mở để mở rộng, đóng để sửa đổi):**  
  * Thêm 100 đồ vật tương tác mới, cửa, tủ, công tắc chỉ cần tạo class mới kế thừa `IInteractable` hoặc `IInspectable`. **Không cần sửa đổi 1 dòng code nào trong Player Controller**.
  * Thêm câu đố mới chỉ cần kế thừa `BasePuzzle` và implement `IPuzzle`.
* **L (Liskov Substitution - Thay thế Liskov):**  
  * Mọi câu đố (`FuseBoxPuzzle`, `PhoneChargingPuzzle`, `RoomEscapeMinigame`) đều kế thừa `BasePuzzle` và có thể được quản lý, reset hoặc kích hoạt thông qua interface `IPuzzle`.
* **I (Interface Segregation - Phân tách giao diện):**  
  * Không tạo một Interface khổng lồ. Tách nhỏ thành:
    * `IInteractable`: Bấm phím tức thời (E).
    * `IHoldInteractable`: Giữ phím có thanh tiến trình (Hold to interact).
    * `IInspectable`: Soi đồ 3D 360 độ.
    * `IHidingSpot`: Điểm chui rúc ẩn nấp.
* **D (Dependency Inversion - Đảo ngược phụ thuộc):**  
  * Các hệ thống bậc cao (Player, Stalker AI, GameManager) không gọi trực tiếp các class con mà giao tiếp thông qua **Interfaces** và **C# Event-Driven Bus** (`IStoryEventService`, `INoiseListener`).

---

## 2.2. Nhóm 1: Hệ Thống Tương Tác (Interaction Contracts & Raycaster)

### `IInteractable.cs`
```csharp
using UnityEngine;

/// <summary>
/// Hợp đồng cho các đồ vật bấm tương tác tức thì (nhấn E)
/// </summary>
public interface IInteractable
{
    string GetInteractionPrompt();
    void Interact(GameObject interactor);
    bool CanInteract(GameObject interactor);
}
```

### `IHoldInteractable.cs`
```csharp
using UnityEngine;

/// <summary>
/// Hợp đồng cho các hành động giữ phím có thời gian (Cạy cửa, Cắt xích, Đập đồ)
/// </summary>
public interface IHoldInteractable : IInteractable
{
    float HoldDuration { get; }
    void OnHoldStart();
    void OnHoldProgress(float normalizedProgress);
    void OnHoldComplete();
    void OnHoldCancel();
}
```

### `IInspectable.cs`
```csharp
using UnityEngine;

/// <summary>
/// Hợp đồng cho đồ vật cầm lên soi 360 độ (CMND, chìa khóa, mẩu giấy nhật ký)
/// </summary>
public interface IInspectable
{
    Transform InspectionAnchor { get; }
    Vector3 InspectRotationOffset { get; }
    void OnInspectStart();
    void OnInspectEnd();
}
```

### Form Skeleton: `PlayerInteractionRaycaster.cs`
```csharp
using System;
using UnityEngine;

/// <summary>
/// Trách nhiệm duy nhất (SRP): Bắn Raycast từ giữa màn hình và kích hoạt các Interface tương tác.
/// </summary>
public class PlayerInteractionRaycaster : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRange = 2.5f;
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private Camera playerCamera;

    // Events decoupled cho UI và Audio tự lắng nghe
    public event Action<IInteractable> OnInteractableFocused;
    public event Action OnInteractableLost;
    public event Action<float> OnHoldProgressUpdated;

    private IInteractable currentInteractable;
    private float currentHoldTimer;
    private bool isHolding;

    private void Update()
    {
        PerformRaycast();
        HandleInteractionInput();
    }

    private void PerformRaycast()
    {
        // TODO: Bắn Raycast từ playerCamera.transform.position theo hướng forward
        // TODO: Nếu trúng collider có IInteractable -> Kích hoạt OnInteractableFocused
        // TODO: Nếu trượt -> Kích hoạt OnInteractableLost
    }

    private void HandleInteractionInput()
    {
        // TODO: Xử lý nhấn phím E (Trigger Interact())
        // TODO: Nếu là IHoldInteractable -> Xử lý đếm thời gian giữ phím và gọi OnHoldProgressUpdated
    }
}
```

---

## 2.3. Nhóm 2: Hệ Thống Cốt Truyện & Event Bus (Decoupled Story Service)

### `StoryFlag.cs`
```csharp
/// <summary>
/// Danh mục cờ cốt truyện toàn cục (loại bỏ hoàn toàn chi tiết thừa, tập trung vào Nam & Nem)
/// </summary>
public enum StoryFlag
{
    // Hồi 1: Đời thường & Điềm báo
    GameStarted,
    LuggageOpened,
    KeyPlacedOnTable,
    WaterBoiling,
    NoodleCooked,
    TVNewsWatched,
    NemCallFinished,
    SeenStrangerInRain,

    // Hồi 2: Thực tại sứt mẻ
    PowerOutageOccurred,
    FlashlightAcquired,
    FuseBoxOpened,
    RedFuseFound,
    YellowFuseFound,
    PowerRestored,
    HalfEatenNoodleSeen,
    CreepyTVBroadcastSeen,
    RoomEscapeMinigameCompleted,
    BathroomSafeRevealed,

    // Hồi 3: Vạch trần & 3 Endings
    PhonePluggedIn,
    PhoneUnlocked,
    LastTenantMessageRead,
    SecretWardrobeOpened,
    SecretWallSpaceEntered,
    NemWarningCallReceived,
    StalkerReturned,
    WardrobeToppled,
    FireExtinguisherAcquired,
    DoorChainCut,
    ComplaintFragmentsFound,
    StalkerHitWithExtinguisher,

    // Endings
    BadEndingReached,
    NormalEndingReached,
    TrueEndingReached
}
```

### `IStoryEventService.cs`
```csharp
using System;

/// <summary>
/// Hợp đồng quản lý và lắng nghe sự kiện tiến trình cốt truyện
/// </summary>
public interface IStoryEventService
{
    event Action<StoryFlag> OnStoryFlagChanged;
    bool HasFlag(StoryFlag flag);
    void SetFlag(StoryFlag flag, bool value);
}
```

### Form Skeleton: `StoryEventBus.cs`
```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trách nhiệm duy nhất: Lưu trữ trạng thái StoryFlag và phát thông báo khi có sự thay đổi.
/// </summary>
public class StoryEventBus : MonoBehaviour, IStoryEventService
{
    public static IStoryEventService Instance { get; private set; }

    public event Action<StoryFlag> OnStoryFlagChanged;
    private readonly HashSet<StoryFlag> activeFlags = new HashSet<StoryFlag>();

    private void Awake()
    {
        // TODO: Khởi tạo Singleton contract
    }

    public bool HasFlag(StoryFlag flag)
    {
        // TODO: Kiểm tra flag có tồn tại trong activeFlags không
        return false;
    }

    public void SetFlag(StoryFlag flag, bool value)
    {
        // TODO: Cập nhật activeFlags và invoke event OnStoryFlagChanged(flag)
    }
}
```

---

## 2.4. Nhóm 3: Khung Giải Đố (Template Method & Open/Closed Puzzles)

### `IPuzzle.cs`
```csharp
using System;

/// <summary>
/// Hợp đồng cho tất cả các câu đố trong game
/// </summary>
public interface IPuzzle
{
    string PuzzleID { get; }
    bool IsSolved { get; }
    event Action<IPuzzle> OnPuzzleSolved;
    void EvaluatePuzzle();
    void ResetPuzzle();
}
```

### `BasePuzzle.cs` (Template Method Pattern)
```csharp
using System;
using UnityEngine;

/// <summary>
/// Lớp cơ sở trừu tượng cho mọi câu đố, xử lý trạng thái chung và bảo đảm nguyên lý OCP/LSP.
/// </summary>
public abstract class BasePuzzle : MonoBehaviour, IPuzzle
{
    [SerializeField] private string puzzleID;
    [SerializeField] private StoryFlag flagToTriggerOnSolve;

    public string PuzzleID => puzzleID;
    public bool IsSolved { get; protected set; }
    public event Action<IPuzzle> OnPuzzleSolved;

    public abstract void EvaluatePuzzle();
    public abstract void ResetPuzzle();

    protected virtual void CompletePuzzle()
    {
        if (IsSolved) return;
        IsSolved = true;
        OnPuzzleSolved?.Invoke(this);

        if (StoryEventBus.Instance != null && flagToTriggerOnSolve != StoryFlag.GameStarted)
        {
            StoryEventBus.Instance.SetFlag(flagToTriggerOnSolve, true);
        }
    }
}
```

### Form Skeleton: `PhoneChargingPuzzle.cs` (Kế thừa `BasePuzzle`)
```csharp
using UnityEngine;

public class PhoneChargingPuzzle : BasePuzzle, IInteractable
{
    [Header("Form Components")]
    [SerializeField] private string correctPasscode = "140898";
    
    // TODO: Khai báo các field UI, dây cáp sạc

    public string GetInteractionPrompt()
    {
        // TODO: Trả về prompt tùy theo trạng thái chưa cắm sạc / đã cắm sạc
        return "";
    }

    public bool CanInteract(GameObject interactor)
    {
        // TODO: Kiểm tra điều kiện có thể tương tác
        return true;
    }

    public void Interact(GameObject interactor)
    {
        // TODO: Mở UI điện thoại, hiển thị bàn phím nhập mật khẩu
    }

    public void SubmitPasscode(string inputCode)
    {
        // TODO: So sánh inputCode với correctPasscode
        // TODO: Nếu đúng -> gọi CompletePuzzle()
    }

    public override void EvaluatePuzzle()
    {
        // TODO: Kiểm tra trạng thái pin và mật khẩu
    }

    public override void ResetPuzzle()
    {
        // TODO: Reset câu đố nếu cần
    }
}
```

### Form Skeleton: `RoomEscapeMinigame.cs` (Meta-Horror Puzzle)
```csharp
using UnityEngine;

public class RoomEscapeMinigame : BasePuzzle
{
    [Header("Meta Synchronization")]
    [SerializeField] private GameObject realWorldMirrorObject;
    [SerializeField] private Transform realWorldMirrorMovedAnchor;

    // TODO: Khai báo Canvas 2D Pixel minigame, vị trí nhân vật pixel

    public void LaunchMinigame()
    {
        // TODO: Khóa di chuyển 3D của Nam, bật Canvas 2D
    }

    public void OnPixelCharacterMovedMirrorIn2D()
    {
        // TODO: Khi nhân vật 2D đẩy gương trong minigame:
        // 1. Đồng bộ đẩy tấm gương thực tế trong phòng tắm 3D
        // 2. Gọi CompletePuzzle()
    }

    public override void EvaluatePuzzle()
    {
        // TODO: Kiểm tra trạng thái gương trong game 2D
    }

    public override void ResetPuzzle()
    {
        // TODO: Reset trạng thái minigame
    }
}
```

---

## 2.5. Nhóm 4: Hệ Thống Ẩn Nấp & Nín Thở (Stealth & Hiding Contracts)

### `IHidingSpot.cs`
```csharp
using UnityEngine;

/// <summary>
/// Hợp đồng cho tủ quần áo, gầm giường
/// </summary>
public interface IHidingSpot
{
    Transform CameraAnchorInside { get; }
    bool IsOccupied { get; }
    void EnterHiding(GameObject player);
    void ExitHiding(GameObject player);
}
```

### `IBreathController.cs`
```csharp
using System;

/// <summary>
/// Hợp đồng quản lý dưỡng khí khi người chơi nín thở
/// </summary>
public interface IBreathController
{
    float CurrentBreathNormalized { get; }
    bool IsHoldingBreath { get; }
    event Action OnSuffocatedAndCoughed;
    void StartHoldingBreath();
    void StopHoldingBreath();
}
```

### Form Skeleton: `HidingController.cs`
```csharp
using UnityEngine;

/// <summary>
/// Quản lý trạng thái trốn và tiêu hao dưỡng khí của Nam
/// </summary>
public class HidingController : MonoBehaviour, IBreathController
{
    [SerializeField] private float maxBreathSeconds = 12f;

    public float CurrentBreathNormalized { get; private set; }
    public bool IsHoldingBreath { get; private set; }
    public event System.Action OnSuffocatedAndCoughed;

    public void StartHoldingBreath()
    {
        // TODO: Bắt đầu trừ dưỡng khí, kích hoạt âm thanh tim đập
    }

    public void StopHoldingBreath()
    {
        // TODO: Ngừng trừ dưỡng khí, hồi phục từ từ
    }

    private void Update()
    {
        // TODO: Đếm ngược thời gian nếu IsHoldingBreath
        // TODO: Nếu hết hơi -> Invoke OnSuffocatedAndCoughed
    }
}
```

---

## 2.6. Nhóm 5: Hệ Thống AI Kẻ Ký Sinh (Perception & State Machine)

### `INoiseListener.cs`
```csharp
using UnityEngine;

/// <summary>
/// Hợp đồng nhận diện âm thanh (DIP: Stalker lắng nghe tiếng động không phụ thuộc trực tiếp vào Player)
/// </summary>
public interface INoiseListener
{
    void OnNoiseHeard(Vector3 noiseOrigin, float loudness);
}
```

### `IStalkerState.cs`
```csharp
/// <summary>
/// Hợp đồng State trong mô hình State Pattern cho Stalker
/// </summary>
public interface IStalkerState
{
    void Enter();
    void Update();
    void Exit();
}
```

### Form Skeleton: `StalkerController.cs`
```csharp
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Điều khiển Stalker FSM: Tuần tra 4 phòng, lắng nghe tiếng bước chân/ho, truy đuổi Nam
/// </summary>
public class StalkerController : MonoBehaviour, INoiseListener
{
    [Header("Navigation")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolWaypoints;

    // TODO: Khai báo các State: PatrolState, SearchState, ChaseState

    public void OnNoiseHeard(Vector3 noiseOrigin, float loudness)
    {
        // TODO: Kiểm tra khoảng cách âm thanh, nếu nghe thấy -> chuyển sang SearchState tại noiseOrigin
    }

    public void SwitchState(IStalkerState newState)
    {
        // TODO: Chuyển đổi trạng thái mượt mà theo State Pattern
    }

    private void Update()
    {
        // TODO: Gọi Update() của State hiện tại
    }
}
```

---

## 2.7. Nhóm 6: Hệ Thống Điện Ảnh & Rung Lắc Camera (Cinemachine & Timeline)

Game kinh dị sống còn nhờ **nghệ thuật máy quay (Camera Direction)**: chuyển cảnh mượt mà từ góc nhìn thứ nhất (FPS) sang góc nhìn điện ảnh, rung màn hình khi sấm nổ (`CinemachineImpulseSource`), và các đoạn Cutscene kịch tính (Ban công Hồi 1, 3 Endings Hồi 3).

### `ICutsceneService.cs`
```csharp
using System;
using UnityEngine;

/// <summary>
/// Hợp đồng quản lý Cutscene và khóa quyền điều khiển người chơi (DIP & SRP)
/// </summary>
public interface ICutsceneService
{
    bool IsPlayingCutscene { get; }
    event Action OnCutsceneStarted;
    event Action OnCutsceneEnded;
    void PlayCutscene(string timelineAssetID, Action onComplete = null);
    void TriggerCameraShake(float impulseForce = 1f);
}
```

### Form Skeleton: `CutsceneController.cs` (PlayableDirector & Cinemachine Impulse)
```csharp
using System;
using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine; // Hoặc Cinemachine trong Unity 2022

/// <summary>
/// Quản lý phát Timeline Cutscenes và phát xung rung chấn Camera (Sấm sét, Tiếng đập cửa, Hất văng)
/// </summary>
public class CutsceneController : MonoBehaviour, ICutsceneService
{
    public static ICutsceneService Instance { get; private set; }

    [Header("Timeline Components")]
    [SerializeField] private PlayableDirector director;

    [Header("Cinemachine Components")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    public bool IsPlayingCutscene => director != null && director.state == PlayState.Playing;
    public event Action OnCutsceneStarted;
    public event Action OnCutsceneEnded;

    private Action pendingOnCompleteCallback;

    private void Awake()
    {
        // TODO: Khởi tạo Singleton contract
    }

    public void PlayCutscene(string timelineAssetID, Action onComplete = null)
    {
        // TODO: Gán pendingOnCompleteCallback
        // TODO: Khóa di chuyển của Player (Invoke OnCutsceneStarted)
        // TODO: Load PlayableAsset tương ứng và gọi director.Play()
    }

    public void TriggerCameraShake(float impulseForce = 1f)
    {
        // TODO: Gọi impulseSource.GenerateImpulse(impulseForce) để rung màn hình
    }

    private void OnTimelineStopped(PlayableDirector aDirector)
    {
        // TODO: Mở khóa di chuyển Player (Invoke OnCutsceneEnded)
        // TODO: Thực thi pendingOnCompleteCallback?.Invoke()
    }
}
```

### 🎬 Danh Sách 5 Phân Cảnh Cutscene & Camera Đắt Giá Nhất:
1. **Cutscene Ban công (Hồi 1):** Timeline điều khiển camera blend từ mắt Nam $\rightarrow$ Zoom cận cảnh bóng người mặc áo mưa đứng bất động dưới mưa $\rightarrow$ Sét đánh giật trắng xóa $\rightarrow$ Blend mượt lại FPS khi Nam giật mình lùi lại.
2. **Impulse Rung chấn (Đầu Hồi 2):** Sấm nổ $\rightarrow$ `TriggerCameraShake(2.0f)` làm màn hình rung bần bật, cốc nước văng đổ lênh láng xuống sàn gạch.
3. **Blend Camera Minigame PC (Hồi 2):** Nam ngồi vào bàn $\rightarrow$ Cinemachine Virtual Camera blend từ tầm mắt người vào trọn khung hình màn hình máy tính CRT.
4. **Timeline Hốc Tường (Hồi 3):** Nam kéo cánh cửa tủ áo $\rightarrow$ Camera lia chậm vào bóng tối hé lộ chiếc đệm rách và đống bao tải nạn nhân.
5. **3 Cutscenes Kết Thúc (Hồi 3):**
   * *Bad Ending:* Camera bị hất văng xuống sàn $\rightarrow$ Fade đen $\rightarrow$ Góc nhìn cận cảnh từ bên trong bao tải bị kéo lê.
   * *Normal Ending:* Camera góc rộng từ trong xe cứu thương nhìn lên ô thông gió tầng 4 dưới mưa bão.
   * *True Ending:* Slow-motion khi giáng bình cứu hỏa $\rightarrow$ Cutscene ánh bình minh rạng rỡ, công an áp giải hung thủ và chủ nhà.

---

# PHẦN III: ROADMAP CHI TIẾT 52 TUẦN (KÈM PLAYLIST VIDEO TUTORIAL)

> 📺 **Bộ tài liệu thực chiến:** Tích hợp trực tiếp danh sách video từ Playlist [Fears To Fathom In Unity](https://youtube.com/playlist?list=PLd8-byumKNmGqb6LllFZT4k5E7CpcYITW) (53 tập) giúp bạn làm đến đâu có video hướng dẫn trực quan đến đó!

---

## 🟢 QUÝ 1: KHUNG CẢNH 4 PHÒNG, VHS FILTER & HỒI 1 ĐỜI THƯỜNG (Tuần 1 - Tuần 13)

### Tuần 1 - 2: Khung Mặt Bằng (Greybox) & Setup Phong Cách VHS URP
* **Công việc kỹ thuật:**
  - Dùng ProBuilder kéo dựng mặt bằng 4 phòng (Khách, Bếp, Ngủ, Toilet) và Ban công.
  - Setup Universal Render Pipeline (URP), cấu hình Global Volume cho phong cách VHS Retro: Film Grain, Chromatic Aberration, Lens Distortion.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 5:** `Fears to Fathom VHS Effect for URP`
  - 🎬 **Video 13:** `Fears to Fathom VHS Camera Effect (Free)`
* **Nghiệm thu:** Đi bộ trong căn hộ xám, nhìn qua camera có filter VHS mờ ảo giống hệt *Fears to Fathom*.

### Tuần 3 - 4: FPS Controller, Flashlight, Camera Zoom & Setup Cinemachine
* **Công việc kỹ thuật:**
  - Cài đặt package **Cinemachine** & **Timeline** từ Unity Package Manager.
  - Cấu hình Cinemachine Virtual Camera (vcam) theo góc nhìn mắt người (Head-bobbing tự nhiên, Field of View 75).
  - Gắn component `CinemachineImpulseSource` lên Main Camera để sẵn sàng tạo xung chấn rung lắc (Camera Shake) cho sấm sét và tiếng đập cửa.
  - Hoàn thiện `PlayerMovement.cs` (Walk, Sprint, Crouch) và `PlayerLook.cs`.
  - Viết `Flashlight.cs` bật tắt đèn pin phím `F`, có Spot Light hotspot trung tâm và hiệu ứng lia đèn mượt.
  - Thêm tính năng phóng to tầm nhìn khi giữ chuột phải (Zoom).
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 25:** `Fears to Fathom Movement System Under 7 Minutes`
  - 🎬 **Video 27:** `Fears to Fathom Flashlight System in Unity Under 20 Min`
  - 🎬 **Video 23:** `Fears To Fathom Camera Zoom System In Unity Under 7 Minutes`
* **Nghiệm thu:** Điều khiển nhân vật trơn tru, bật đèn pin soi rõ từng góc phòng tối, zoom camera mượt mà, sẵn sàng hệ thống máy quay điện ảnh.

### Tuần 5 - 6: Hệ Thống Tương Tác Raycast, Tâm Ngắm & Cầm Nắm
* **Công việc kỹ thuật:**
  - Viết `IInteractable.cs` và `PlayerInteraction.cs` bắn Raycast 2.5m từ giữa màn hình.
  - Tâm ngắm động thay đổi biểu tượng khi nhìn vào đồ vật có thể tương tác (chìa khóa, cánh cửa, công tắc đèn).
  - Viết script mở cửa phòng (xoay góc 90 độ mượt), kéo mở ngăn kéo bàn.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 36, 38, 39:** `Fears to Fathom Interaction System in Unity (Part 1 - 3)`
  - 🎬 **Video 44:** `Fears To Fathom Dynamic Crosshair In Unity`
  - 🎬 **Video 35, 37:** `Fears to Fathom Grab Drop Throw Mechanic in Unity (Part 1 - 2)`
* **Nghiệm thu:** Mở được toàn bộ các cánh cửa, ngăn kéo trong nhà; tâm ngắm phản hồi chính xác.

### Tuần 7 - 8: Soi Đồ Vật 3D (Inspect) & Thanh Tiến Trình (Hold Progress Bar)
* **Công việc kỹ thuật:**
  - Viết `InspectManager.cs`: Nhặt mẩu giấy nhật ký, chìa khóa, điện thoại đưa lại gần camera và xoay 360 độ bằng chuột.
  - Làm thanh tròn hiển thị tiến trình khi giữ phím tương tác lâu (ví dụ giữ `E` để mở khóa chốt).
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 46, 47, 48:** `Fears To Fathom Progress Bar In Unity (Part 1 - 3)`
* **Nghiệm thu:** Cầm được bức thư lên xoay đọc nội dung 2 mặt, đặt lại vị trí cũ bằng phím `ESC`.

### Tuần 9 - 10: Trái Tim Cốt Truyện `StoryFlagManager` & Tin Nhắn Nem
* **Công việc kỹ thuật:**
  - Thiết lập `StoryFlagManager.cs` đồng bộ toàn bộ logic nhiệm vụ.
  - Làm khung UI tin nhắn điện thoại với Nem: Hiệu ứng chữ chạy (Typewriter effect) kèm âm thanh gõ lách cách chân thực.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 15, 16:** `Fears to Fathom Dialogue Background (Method 01 & 02)`
  - 🎬 **Video 49 - 53:** `Fears To Fathom Dialogue System In Unity (Part 1 - 5)`
* **Nghiệm thu:** Chuông điện thoại reo, mở lên đọc cuộc đối thoại ấm áp của Nem dặn dò Nam đóng cửa đi ngủ.

### Tuần 11 - 12: Chuỗi Việc Nhà Hồi 1 & Cutscene Ban Công (Timeline + Cinemachine)
* **Công việc kỹ thuật:**
  - Lập trình chuỗi việc nhà: Cầm ấm lấy nước bồn rửa $\rightarrow$ Bật bếp gas $\rightarrow$ Nước sôi $\rightarrow$ Rót vào bát mì tôm $\rightarrow$ Cầm đũa ăn mì.
  - Lập trình bật Tivi xem bản tin thời sự cảnh báo bão lớn.
  - **Dàn dựng Cutscene Ban công bằng Unity Timeline & Cinemachine:**
    - Khi Nam kéo cửa kính ra ban công: Khóa di chuyển, kích hoạt Timeline.
    - Cinemachine Virtual Camera chuyển góc quay từ từ zoom xuống mặt sân đẫm nước mưa $\rightarrow$ Bắt trọn bóng người mặc áo mưa đứng bất động ngửa mặt nhìn lên.
    - Sấm sét nổ đinh tai, màn hình giật lóa trắng xóa $\rightarrow$ Bóng người biến mất $\rightarrow$ Camera blend mượt mà lại góc nhìn mắt Nam giật mình thở dốc lùi vào trong phòng.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 26, 31, 32, 33:** `Fears to Fathom Food & Eating System in Unity (Part 1 - 3)`
  - 🎬 **Video 30:** `Fears to Fathom Coffee Drinking Mechanic in Unity`
  - 🎬 **Video 12:** `Fears to Fathom TV`
* **Nghiệm thu:** Hoàn chỉnh toàn bộ chuỗi hành động ấm cúng đời thường và đoạn Cutscene điềm báo lạnh gáy của Hồi 1.

### Tuần 13: Cột Mốc Review Hồi 1 (Milestone 1 Playthrough)
* **Nghiệm thu:** Playtest liền mạch 20 phút đầu game. Không còn lỗi logic kẹt cờ nhiệm vụ, 60 FPS ổn định.

---

## 🟡 QUÝ 2: THỰC TẠI SỨT MẺ, MINIGAME PC, KÉT SẮT TOILET & DEMO 25 PHÚT (Tuần 14 - Tuần 26)

### Tuần 14 - 15: Biến Cố Mất Điện, Rung Lắc Camera & Câu Đố Cầu Dao
* **Công việc kỹ thuật:**
  - Âm thanh sấm nổ đinh tai $\rightarrow$ Kích hoạt `CinemachineImpulseSource` làm màn hình rung lắc bần bật giật bắn người.
  - Toàn bộ đèn nhà phụt tắt, cốc nước bị gạt đổ lênh láng ra sàn gạch.
  - Lần mò bật đèn pin, vào phòng kho mở hộp cầu dao (Fuse Box) thấy thiếu cầu chì đỏ và vàng.
  - Đi tìm cầu chì đỏ (gầm giường phòng ngủ) và vàng (tủ bếp); đọc các trang nhật ký người thuê cũ (nghe tiếng thở trong ống gió).
  - Đặt chiếc bình chữa cháy mini han gỉ ở góc kho làm điểm nhấn (Chekhov's Gun).
* **Nghiệm thu:** Cắm đủ 2 cầu chì, kéo cần gạt, đèn nhà bật sáng trở lại.

### Tuần 16: Sự Méo Mó Rùng Rợn & Tivi Camera Giấu Kín
* **Công việc kỹ thuật:**
  - Khi điện bật sáng: Bát mì trên bàn đã bị ăn mất một nửa! Cạnh bên xuất hiện một cốc nước mới uống dở!
  - Tivi tự động bật: Chuyển kênh sang phát trực tiếp hình ảnh đen trắng quay chính Nam từ camera sau bức tranh!
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 12:** `Fears to Fathom TV` (Dùng RenderTexture lấy hình ảnh từ Camera phụ giấu sau tranh).
* **Nghiệm thu:** Người chơi nhìn vào tivi thấy chính lưng mình đang đứng trong phòng khách $\rightarrow$ Nổi da gà cực độ!

### Tuần 17 - 18: Blend Camera Vào PC Minigame `RoomEscape.exe` & Két Sắt Toilet
* **Công việc kỹ thuật:**
  - Dùng **Cinemachine Virtual Camera** chuyển cảnh: Khi Nam ngồi vào bàn làm việc, camera blend mượt mà từ tầm mắt người vào trọn khung màn hình CRT.
  - Viết `RoomEscapeMinigame.cs`: Minigame 2D Pixel trên máy tính mô phỏng căn phòng của Nam.
  - Người chơi điều khiển nhân vật pixel đẩy gương toilet làm lộ két sắt trong game.
  - Khi người chơi rời máy tính, ra toilet thực tế đẩy gương: Chiếc két sắt âm tường thật sự nằm ở đó!
* **Nghiệm thu:** Cú twist Meta-Horror kiểu *MiSide* hoạt động đồng bộ hoàn hảo giữa 2D Canvas và 3D Scene kèm máy quay Cinemachine mượt mà.

### Tuần 19 - 20: Câu Đố Sạc Pin iPhone & Mở Khóa Mật Khẩu
* **Công việc kỹ thuật:**
  - Viết `PhoneChargingPuzzle.cs`: Mở két sắt lấy CMND bị gạch mặt, chìa khóa "Tủ áo", điện thoại sập nguồn.
  - Tìm dây cáp sạc cắm cho điện thoại $\rightarrow$ Nhập ngày sinh trên CMND $\rightarrow$ Đọc tin nhắn: *"Có ai đó đang sống trong tủ áo..."*.
* **Nghiệm thu:** Nhập đúng mật khẩu mở khóa điện thoại, đọc toàn bộ tin nhắn lưu trữ của nạn nhân cũ.

### Tuần 21 - 22: Đắp Asset 3D Chung Cư Cũ & Ánh Sáng URP Hoàn Chỉnh
* **Công việc kỹ thuật:**
  - Thay thế toàn bộ khối xám ProBuilder bằng Model 3D chung cư cũ Việt Nam (tường vôi vàng, gạch bông, cửa gỗ cũ).
  - Tinh chỉnh ánh sáng đèn dây tóc vàng và thanh điều chỉnh độ sáng màn hình (Gamma Slider).
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 22:** `Fears To Fathom Gamma Slider In Unity`
* **Nghiệm thu:** Toàn bộ căn hộ đạt chất lượng đồ họa Retro-Realism như một bộ phim kinh dị thực thụ.

### Tuần 23 - 24: Cơ Chế Nín Thở & Ẩn Nấp (`HideAndSeekManager`)
* **Công việc kỹ thuật:**
  - Viết `HideAndSeekManager.cs`: Cho phép người chơi chui vào trốn trong tủ quần áo hoặc chui gầm giường.
  - Cơ chế giữ chuột phải nín thở, thanh dưỡng khí giảm dần, nếu hết hơi sẽ ho sặc sụa báo động cho kẻ thù.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 17, 18, 19:** `Fears to Fathom Hiding Mechanic (Part 1 - 3)`
* **Nghiệm thu:** Trốn vào tủ áo nhìn qua khe hở, nín thở nghe tiếng tim đập dồn dập.

### Tuần 25 - 26: Đóng Gói Bản DEMO 25 Phút Hoàn Chỉnh
* **Nghiệm thu:** Build ra file `.exe` Windows hoàn chỉnh từ mở vali đến khi mở két sắt toilet để chuẩn bị cho Playtest.

---

## 🟠 QUÝ 3: HỐC TƯỜNG BÍ MẬT, RƯỢT ĐUỔI HỒI 3 & 3 ENDINGS (Tuần 27 - Tuần 39)

### Tuần 27 - 28: Playtest Bản Demo & Tinh Chỉnh (Feedback Loop)
* **Công việc:** Gửi file Demo cho 10 - 15 bạn bè/người chơi thử. Ghi nhận những chỗ bị kẹt câu đố hoặc giật lag để sửa ngay.

### Tuần 29 - 30: Hốc Tường Sau Tủ Áo & Cuộc Gọi Báo Động Của Nem
* **Công việc kỹ thuật:**
  - Dùng chìa khóa mở tủ áo, gạt đồ sang bên phát hiện vách ngăn bí mật dẫn vào khoảng không giữa 2 lớp tường.
  - Bố trí hiện trường: Chiếc đệm bẩn, vỏ mì tôm, màn hình nối camera sau tranh, và các bao tải lớn chứa đồ nạn nhân cũ.
  - Lập trình cuộc gọi của Nem từ sảnh: Bác bảo vệ báo phòng này từng có án mạng bị niêm phong, chủ nhà ham cọc cho thuê lậu.
* **Nghiệm thu:** Khám phá căn phòng bí mật mang lại cảm giác nghẹt thở và ghê tởm chân thực.

### Tuần 31 - 32: Kẻ Thủ Ác Lộ Diện, Rượt Đuổi & Stalker AI
* **Công việc kỹ thuật:**
  - Tiếng sột soạt: Kẻ ký sinh chui từ thông gió về tổ $\rightarrow$ Nam đạp đổ tủ áo chặn cửa rồi lao ra ngoài.
  - Cửa chính bị khóa chốt xích từ bên trong.
  - Viết `StalkerAI.cs`: Kẻ đột nhập đi tuần tra quanh 4 phòng, lắng nghe tiếng động, truy đuổi khi nhìn thấy Nam.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 40, 41, 42:** `Fears To Fathom NPC In Unity (Part 1 - 3)`
  - 🎬 **Video 43, 45:** `Fears To Fathom Jumpscare In Unity (Part 1 - 2)`
* **Nghiệm thu:** Cuộc rượt đuổi nghẹt thở trong không gian hẹp, AI phản ứng cực nhạy với tiếng bước chân chạy.

### Tuần 33 - 34: Lập Trình Chi Tiết 3 Nhánh Kết Thúc (3 Endings)
* **Công việc kỹ thuật:**
  - **Ending 1 (Bad - Cái Bao Tải Thứ Tư):** Bị bắt $\rightarrow$ Màn hình tối sầm $\rightarrow$ Góc nhìn từ trong bao tải bị kéo lê trên sàn.
  - **Ending 2 (Normal - Tiếng Cửa Trong Mưa):** Tìm kìm cộng lực dưới bồn rửa, cắt xích cửa lao ra gặp Nem và bảo vệ.
  - **Ending 3 (True/Secret - Mọi Chuyện Sáng Tỏ):**
    - Nem đã chủ động gọi Cảnh sát 113 từ sảnh ngay từ đầu.
    - Nam nhặt đủ 3 mảnh đơn khiếu nại (vạch trần chủ nhà), lấy bình cứu hỏa ở kho, trốn vào toilet.
    - Kẻ địch phá cửa $\rightarrow$ Nam xịt mù mắt rồi **đập bình cứu hỏa vào đầu kẻ địch** ngã gục vào bồn tắm $\rightarrow$ Khóa cửa toilet.
    - Công an cùng Nem ập vào bắt sống kẻ sát nhân và bắt giữ gã chủ nhà gian ác. Trời sáng, bão tan!
* **Nghiệm thu:** Cả 3 kết thúc đều được kích hoạt trơn tru theo đúng cờ logic và điều kiện người chơi thực hiện.

### Tuần 35 - 36: Lồng Tiếng Thoại & Bản Địa Hóa Song Ngữ (Anh - Việt)
* **Công việc:**
  - Thu âm lồng tiếng (Voice Acting) cho Nam, Nem, bác bảo vệ và tiếng thở thì thào bệnh hoạn của kẻ sát nhân.
  - Bản địa hóa toàn bộ giao diện, phụ đề, nhật ký sang tiếng Anh chuẩn để bán quốc tế.

### Tuần 37 - 38: Hệ Thống Menu Hoàn Chỉnh & Tối Ưu Hóa 60 FPS
* **Công việc kỹ thuật:**
  - Làm Main Menu chuẩn phong cách *Fears to Fathom*: Chọn Chapter, Checkpoint, Cài đặt âm thanh, Đổi ngôn ngữ.
  - Nướng Occlusion Culling, tối ưu ánh sáng đảm bảo game chạy mượt 60 FPS trên card GTX 1050Ti / GTX 1650.
* **Tutorial tham khảo từ Playlist:**
  - 🎬 **Video 6 - 11:** `Fears to Fathom Main Menu (Part 1 - 6)` (UI, Settings, Animations, Checkpoints & Scene Select, Change Language).
* **Nghiệm thu:** Menu hoạt động mượt mà, lưu checkpoint chuẩn xác, game load nhanh dưới 5 giây.

### Tuần 39: Nghiệm Thu Full Build v1.0-RC (Release Candidate)
* **Nghiệm thu:** Hoàn tất 100% nội dung Chapter 1 với thời lượng chơi trọn vẹn 60 - 90 phút.

---

## 🔴 QUÝ 4: STEAMWORKS, MARKETING STREAMER & PHÁT HÀNH (Tuần 40 - Tuần 52)

### Tuần 40 - 41: Đăng Ký Steamworks & Thiết Kế Capsule Art
* **Công việc:**
  - Nộp phí Steam Direct ($100), nhận Steam App ID.
  - Thiết kế bộ ảnh đại diện bìa game (Capsule Art) thu hút thị giác: Khe cửa tủ áo hé mở với ánh mắt rình mò trong bóng tối.

### Tuần 42 - 43: Dựng Trailer 60 Giây & Mở Công Khai Steam Store Page
* **Công việc:**
  - Cắt dựng video Trailer 60 giây nghẹt thở theo phong cách found-footage VHS.
  - Mở trang Steam Store ở trạng thái "Coming Soon" để bắt đầu tích lũy Wishlist.

### Tuần 44 - 45: Phát Hành Steam Demo & Gửi Key Streamer Đợt 1
* **Công việc:**
  - Tung bản Demo miễn phí 20 phút lên Steam.
  - Gửi email kèm key Demo cho danh sách 30 streamer chuyên chơi indie horror (Việt Nam & Quốc tế).

### Tuần 46 - 47: Bùng Nổ Wishlist Với Steam Next Fest & Valve Review
* **Công việc:**
  - Tham gia lễ hội Steam Next Fest, phát livestream gameplay vòng lặp liên tục để đẩy Wishlist vượt mốc 5,000+.
  - Gửi bản Full Build hoàn chỉnh cho đội ngũ Valve kiểm duyệt lần cuối.

### Tuần 48 - 49: Gửi Key Bản Full & BẤM NÚT PHÁT HÀNH CHÍNH THỨC!
* **Công việc:**
  - Gửi key bản đầy đủ cho các YouTuber/Streamer đã chơi bản demo.
  - **BẤM NÚT RELEASE CHÍNH THỨC TRÊN STEAM!** Game chính thức mở bán toàn cầu.

### Tuần 50 - 52: Hậu Phát Hành, Cập Nhật Hotfix & Lên Ý Tưởng Chapter 2
* **Công việc:**
  - Theo dõi review của game thủ, trực chiến cập nhật bản vá nóng (Hotfix) nếu có bug.
  - Lên concept cho Chapter 2 (Bối cảnh quán net đêm hoặc trạm xe buýt ven quốc lộ, tận dụng hệ thống xe hơi từ Video 1-4 trong Playlist).

---

---

# PHẦN IV: CHIẾN LƯỢC ĐẶT TÊN SERIES & MỞ RỘNG NHIỀU CHAPTER

## 4.1. Có nên đổi tên game không?
**CÂU TRẢ LỜI LÀ: RẤT NÊN ĐỔI!**

Cái tên ban đầu `Project Uncanny Room` mang tính kỹ thuật nội bộ. Khi chuyển sang mô hình Series Tuyển tập nhiều chương (Anthology), bạn nên đặt theo công thức chuẩn quốc tế:

$$\text{[Tên Thương Hiệu Series]} : \text{[Tên Riêng Của Từng Chapter]}$$

### 3 Phương án Tên Series Đắt Giá Nhất:

1. **Phương án 1 (Hiện đại, Hút Streamer Quốc Tế - Giống *Fears to Fathom*):**
   * **Tên Series:** **Urban Dread** *(Nỗi Sợ Đô Thị)*
   * **Tập 1:** **Urban Dread: The Fourth Bag** *(Chiếc Bao Tải Thứ Tư)*
   * *Ý tưởng cho các tập sau:*
     * Tập 2: *Urban Dread: The Night Shift Netcafe* (Quán Net Ca Đêm)
     * Tập 3: *Urban Dread: Highway Motel* (Nhà Nghỉ Ven Quốc Lộ)

2. **Phương án 2 (Đậm Chất Huyền Bí Việt Nam - Giống *Thần Trùng*):**
   * **Tên Series:** **Dị Sự Nửa Đêm** *(Midnight Tales)*
   * **Tập 1:** **Dị Sự Nửa Đêm: Căn Hộ Tầng 4**
   * *Ý tưởng cho các tập sau:*
     * Tập 2: *Dị Sự Nửa Đêm: Chuyến Xe Buýt Cuối Cùng*
     * Tập 3: *Dị Sự Nửa Đêm: Nhà Trọ Cuối Hẻm*

3. **Phương án 3 (Phong cách Meta-Horror / Không Gian Bị Xâm Chiếm - Giống *MiSide*):**
   * **Tên Series:** **The Liminal Room** *(Căn Phòng Méo Mó)*
   * **Tập 1:** **The Liminal Room: Behind The Wall** *(Phía Sau Bức Tường)*

---

*Toàn bộ nội dung của bản kế hoạch này đã được đồng bộ hóa và lưu trữ trực tiếp vào file hệ thống của bạn để bạn có thể mở trong Word bất cứ lúc nào!*
