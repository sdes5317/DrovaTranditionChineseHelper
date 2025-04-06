# Drova 漢化練習
從`resources.assets`可以判斷這款遊戲是unity, 這款遊戲漢化有兩個步驟.
### 1. 更換翻譯
使用bash指令查看resources.assets的版本來使用拆包工具
```
xxd -l 64 resources.assets
```
大概是第49個到59個bytes轉成字串為`2022.3.45f1`  
請Grok搜尋了一下, 找到這款可以支援拆包(https://github.com/hengtek/AssetStudio22/tree/master)  
不過需要自行編譯一下  
打開AssetStudio22後, 試著搜尋一下Font的資源  
可以發現他的字型檔在這`aline_font`, 導出這個檔案在windows直接打開, 確認他使用的字型`Droid Sans Mono`不支援繁體  
這時可以改用簡體漢化釋出的`Source Han Sans CN`(或是自己找一個支援繁體字的)來取代  
(把要使用的新字幕檔改為`aline_font`在透過AssetStudio22導回resources.assets)

### 2. 補上字幕
由於之前已有繁體漢化 只是版本不對
這邊用程式把舊有的漢化覆蓋到新的版本上
這個目錄下loc的檔案都可以用文字編輯器打開
```
steamapps\common\Drova - Forsaken Kin\Drova_Data\StreamingAssets\Localization
```
(新的字幕有空再來補翻...)
