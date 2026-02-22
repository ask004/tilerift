# Unity Task Board (7 Gün / MVP)

Toplam tahmin: `52-60 saat` (tek geliştirici)

## Backlog (Issue Listesi + Saat)

1. `INIT-01` Proje kurulumu ve klasör yapısı  
Tahmin: `2s`  
Kabul kriteri: `Scenes`, `Scripts`, `Prefabs`, `Data`, `UI` yapısı hazır.

2. `CORE-01` Grid/Board veri modeli  
Tahmin: `3s`  
Kabul kriteri: Hücreler, parça tipi, doluluk kontrolü çalışıyor.

3. `CORE-02` Drag & Drop sistemi  
Tahmin: `4s`  
Kabul kriteri: Parça seçme/taşıma/bırakma stabil.

4. `CORE-03` Match/Sort kural motoru  
Tahmin: `4s`  
Kabul kriteri: Geçerli/Geçersiz hamle kontrolü doğru.

5. `CORE-04` Win/Lose state yönetimi  
Tahmin: `2s`  
Kabul kriteri: Bölüm bitiş ve başarısızlık net tetikleniyor.

6. `LEVEL-01` JSON level formatı ve loader  
Tahmin: `3s`  
Kabul kriteri: JSON’dan level runtime’da yükleniyor.

7. `LEVEL-02` Level flow (start/restart/next)  
Tahmin: `2s`  
Kabul kriteri: Akış kesintisiz, soft lock yok.

8. `UI-01` HUD (hamle, coin, pause)  
Tahmin: `3s`  
Kabul kriteri: HUD doğru veri gösteriyor.

9. `UI-02` Menü ekranları (home, fail, win)  
Tahmin: `3s`  
Kabul kriteri: Tüm ekran geçişleri stabil.

10. `POLISH-01` Temel animasyonlar ve feedback  
Tahmin: `4s`  
Kabul kriteri: Move/success/fail hissi yeterli.

11. `CONTENT-01` 60 level üretimi  
Tahmin: `8s`  
Kabul kriteri: Zorluk eğrisi kolay-orta-zor dağıtılmış.

12. `CONTENT-02` Dengeleme turu (playtest fix)  
Tahmin: `4s`  
Kabul kriteri: İlk 15 dk’da sert tıkanma yok.

13. `DAILY-01` Daily Challenge seed sistemi  
Tahmin: `3s`  
Kabul kriteri: Her gün tek bölüm deterministik üretiliyor.

14. `DAILY-02` Streak ve günlük ödül  
Tahmin: `2s`  
Kabul kriteri: Günlük girişte streak artıyor/bozuluyor doğru.

15. `ECON-01` Coin ekonomisi (kazan/harca)  
Tahmin: `2s`  
Kabul kriteri: Coin loop çalışıyor, negatif coin yok.

16. `BOOST-01` Boosters (Undo/Hint/Shuffle - basit)  
Tahmin: `4s`  
Kabul kriteri: 3 booster temel haliyle çalışır.

17. `MON-01` AdMob rewarded entegrasyonu  
Tahmin: `3s`  
Kabul kriteri: Fail sonrası continue ve coin ödülü çalışır.

18. `MON-02` AdMob interstitial entegrasyonu  
Tahmin: `2s`  
Kabul kriteri: Frekans kuralı ile reklam gösterimi stabil.

19. `MON-03` Unity IAP ürünleri  
Tahmin: `3s`  
Kabul kriteri: `No Ads` + `Coin Pack` purchase flow tamam.

20. `AN-01` Analytics eventleri  
Tahmin: `2s`  
Kabul kriteri: `level_start/complete`, `ad_watched`, `iap_purchase` loglanır.

21. `QA-01` Android build pipeline + test  
Tahmin: `2s`  
Kabul kriteri: Release build alınır, kritik crash yok.

22. `QA-02` Bugfix buffer  
Tahmin: `3s`  
Kabul kriteri: P0/P1 bug kapanmış.

23. `STORE-01` Store assetleri (ikon, 5 screenshot, kısa video)  
Tahmin: `3s`  
Kabul kriteri: Yayın için minimum materyal hazır.

24. `STORE-02` Store listing metni + ASO  
Tahmin: `2s`  
Kabul kriteri: Başlık, kısa/uzun açıklama, keyword set tamam.

## Gün Bazlı Sprint Dağılımı

1. Gün 1: `INIT-01`, `CORE-01`, `CORE-02`, `CORE-03` (`13s`)
2. Gün 2: `CORE-04`, `LEVEL-01`, `LEVEL-02`, `UI-01`, `UI-02` (`12s`)
3. Gün 3: `POLISH-01`, `CONTENT-01` (ilk 40 level), `CONTENT-02` (ilk tur) (`12s`)
4. Gün 4: `CONTENT-01` (kalan 20), `DAILY-01`, `DAILY-02`, `ECON-01` (`9s`)
5. Gün 5: `BOOST-01`, `MON-01`, `MON-02`, `MON-03` (`12s`)
6. Gün 6: `AN-01`, `QA-01`, `QA-02` (`7s`)
7. Gün 7: `STORE-01`, `STORE-02`, final regression (`5s`)

## Definition of Done (MVP)

- 60 level oynanabilir.
- Daily challenge + streak aktif.
- Rewarded + interstitial + 2 IAP sorunsuz.
- Android release build store’a yüklenebilir.
- Kritik crash/soft lock yok.
