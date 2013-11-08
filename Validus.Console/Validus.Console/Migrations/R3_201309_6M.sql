﻿CREATE TABLE [dbo].[QuotesME] (
    [Id] [int] NOT NULL,
    [AmountOrOPL] [nvarchar](max),
    [AmountOrONP] [nvarchar](max),
    [LineSize] [decimal](18, 2),
    CONSTRAINT [PK_dbo.QuotesME] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Id] ON [dbo].[QuotesME]([Id])
CREATE TABLE [dbo].[QuotesHM] (
    [Id] [int] NOT NULL,
    [AmountOrOPL] [nvarchar](max),
    [AmountOrONP] [nvarchar](max),
    [VesselTopLimitCurrency] [nvarchar](10),
    [VesselTopLimitAmount] [decimal](18, 2),
    CONSTRAINT [PK_dbo.QuotesHM] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Id] ON [dbo].[QuotesHM]([Id])
CREATE TABLE [dbo].[QuotesCA] (
    [Id] [int] NOT NULL,
    [AmountOrOPL] [nvarchar](max),
    [AmountOrONP] [nvarchar](max),
    [LineSize] [decimal](18, 2),
    CONSTRAINT [PK_dbo.QuotesCA] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Id] ON [dbo].[QuotesCA]([Id])
CREATE TABLE [dbo].[OptionVersionsHM] (
    [OptionId] [int] NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [TSICurrency] [nvarchar](10),
    [TSIAmount] [decimal](18, 2),
    CONSTRAINT [PK_dbo.OptionVersionsHM] PRIMARY KEY ([OptionId], [VersionNumber])
)
CREATE INDEX [IX_OptionId_VersionNumber] ON [dbo].[OptionVersionsHM]([OptionId], [VersionNumber])
CREATE TABLE [dbo].[OptionVersionsCA] (
    [OptionId] [int] NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [TSICurrency] [nvarchar](10),
    [TSIAmount] [decimal](18, 2),
    CONSTRAINT [PK_dbo.OptionVersionsCA] PRIMARY KEY ([OptionId], [VersionNumber])
)
CREATE INDEX [IX_OptionId_VersionNumber] ON [dbo].[OptionVersionsCA]([OptionId], [VersionNumber])
ALTER TABLE [dbo].[QuotesME] ADD CONSTRAINT [FK_dbo.QuotesME_dbo.Quotes_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Quotes] ([Id])
ALTER TABLE [dbo].[QuotesHM] ADD CONSTRAINT [FK_dbo.QuotesHM_dbo.Quotes_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Quotes] ([Id])
ALTER TABLE [dbo].[QuotesCA] ADD CONSTRAINT [FK_dbo.QuotesCA_dbo.Quotes_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Quotes] ([Id])
ALTER TABLE [dbo].[OptionVersionsHM] ADD CONSTRAINT [FK_dbo.OptionVersionsHM_dbo.OptionVersions_OptionId_VersionNumber] FOREIGN KEY ([OptionId], [VersionNumber]) REFERENCES [dbo].[OptionVersions] ([OptionId], [VersionNumber])
ALTER TABLE [dbo].[OptionVersionsCA] ADD CONSTRAINT [FK_dbo.OptionVersionsCA_dbo.OptionVersions_OptionId_VersionNumber] FOREIGN KEY ([OptionId], [VersionNumber]) REFERENCES [dbo].[OptionVersions] ([OptionId], [VersionNumber])
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201309271336238_R3_201309_6', 0x1F8B0800000000000400ED7DCB72E43892E07DCDF61F643ACD8CD9A452395D6DD56D9933A66795ACF588959499567309A32220899B116414C9C84AF5AFED613F697F6101BE01389E044846144F521080C3E170381C0E87FBFFFB3FFFF7E37FFD58AF0EBEA3240DE3E8D3E1F1BBF78707285AC4CB307AF974B8CD9EFFFDE7C3FFFACFFFF93F3E5E2CD73F0EBE54F5FE83D4C32DA3F4D3E16B966DFE7E74942E5ED13A48DFADC34512A7F173F66E11AF8F82657CF4E1FDFBBF1D1D1F1F210CE210C33A38F878BF8DB2708DF21FF8E7591C2DD026DB06AB9B78895669F91D973CE4500F6E83354A37C1027D3AFC12ACC2E5367D87DBA4F10ABD3B0FB2E0F0E0641506189707B47A3E3CD8FCE5EF9F53F4902571F4F2B009B230583DBE6D102E7F0E56292A31FFFBE62FBAC8BFFF40903F0AA228CE30B838B21AFC613D2C3CB00B4C80EC8DA0950F0EA3BE7D5A8729216FBB1EAEF90FF4467DC09F6649BC4149F6768F9ECBD657CBC38323BADD11DBB06ED66A4310C0FF45D97F7C383CB8DDAE56C1D30AD574C2847CC8E204FD82229404195ACE822C434944DAA27C005CAF4C1F8F61B64255377846305B1D1E5C863FD0F21A452FD96BDDD54DF0A3FA72FC1E33D7E728C45C881B65C91601A8C9BB3D47E9220937C55419758EFF95745EFC96F77D15A5DB042DC98FFE075E76AE9E5A3998D324FE8692338C87F110BA8EA0E87A96A2ED328EDED603F5FF807EDF6239885C1132CA82456638969F6463D1E1C4DB38BA8E234C463FD3698182F345A183C3E76889923F92301B88A3A9FE734618048DFFB58D33DCD3DDF373D8E66BCDFE3FFCF4D7CE4239C6DB6368B11D74EDF81A057806FA66FD82E383977ABCE76811AE83D5E1C12CC1FF950AD7CF87070F8B80C0FC60DA019950F4F08A50768BFF497BDFE85A7C3D0C028F58854CB360BDA97A3E0DA320C13A09D127B7498205F8DB4D0E3D474589D1CF462AD059BCDE6C33C42B5E0C968D624790345E782E1412AC2F67842E662BBEEB027808B1466FA18275EEF82E692D77D7ABEE22CD1EC29768B6C85E7C75719620C2697735E1F019071166370584993F7C0E5D402A513A356622E9B66132863EBABE0DBE872F39CB8A45DDE1C13D5AE575D2D770531C39DF356B7C4E55BD4CE2F57DBCA2A440BBC6FC21DE260BB243C4D26A8F41F282322B7C6BDD531BEDBA850AFBB2A2E620AADAA663A1F416D53098CAE008A83A32E4E98AA678DFE5C7CF14C4B8289BB7CEFC2D54B9C2BAEB0A47BE46350A5DE46E82E41BCABEC60931F43CA08C8C1346B5D589A0114864B0AE8CD8700353A29FE19D2E5E5B8D4EDA141CA3A4856CA4B266A6E37D44C93ABDC54B6B199252C321AB5A83A39637920D5CD1D26EAEBB51400F8664F6EDA9A1D5DE942618FEFF468BEC313E5B05DB14195243D51AA483BC918C028A9676FCD08D027A3024FC604F0DADF6104D3E1E35C664A989B9AD9A58D8980BAB85A99579105B87859DA9BBA63AA9ED43A8EDDADC5FEA84BD5FAEF4676073CEF55ABDDA5CF138E8F6328E3363A3DEB4C8777391CBB6FD93E53241A9E09C551CDDEA2AAD431655C2EDCB4C71A76DB7EA7D8FAE75CB215D87113AEE9DEB5A9D7FE87FFDE7D4E9B9D3870CCF12FEF63D8C16E81EBD58D8543BE3F0DFE16616A759B0B250E83A5B74CFE26D6461C1EE3CE8D96B1CF5AF485E12937FCF7D7E4E56D35EFAA7D84BF515E6C27F668FB6ADE6B4DDD5AF6320A5F72C5EAFF150A7ABD649A08C47A028EC91F50D47F77B104E4B17DE94985DD2947EADB2BB9AB2CABC148A1C9A74B9E0C686A9045DDA184AE712A28D902E00F0A21AAC5CF673BB5D3F9143B7A9706FFAEA2274192426F96DE003925EFCC0BF43D27B2DC263CC9A4164EE639A5EC78B6F68D915CEB4A74C7B8ACD9E52896057B21ADE550402DDC46B01C11B4A5E34A7E5770B47A098DB4EA03AA62E008DFBA004CDBC7CCEEE920CB250251865B066A76D3007BB4F6794D16D95673116C505773CC6DF502DBD7ED9864BF36D08EB6AE469C613BA09D296FDDE760F398B37581C117ECC194142362D3B5B851C661344F61F5315A1B3CBFE3D8A66F1CAF8F2A873BF97C1225C85F9FAE8BBEB467B2766CEADA96E24ED5F8B872E886971A0BEF164878B5C9EF5DEF55D12BE60856BD837016777A7C6DD76770FB8B93BE9BDCF93C58218B17F434147797C95BF95C4CB85A88F5D55497C2A089337179072F9AB074EEF748016AF98E0C16A96840B3C4537287B8D9DB28A96DD8BC1021F519656A2C23926B304ADC3ED9AF8A19FAC4D9F96B9C42647C39B2B7C790EEC7B7CD7E13ACCCECE7EEBBBDF8B1F0B94A603747C8AA9FCBA0E926F9E273417134BCF9DE4B377B226F2D6DB33907CA2FCF6515A8B7A3716E171AC8866B04DB0221CA4035C36B73098AE3CF48E2C1CBA58BBF8EB5FA62BD9FDB076D5762237F624D6E425B3396923CA9E2764C8F2753984D92A22A4B97AC67EE377A71254F3521639FC51800E29E9E42D46FA1BAB8FAA0BC7EC04CBB9F0BBE971D741688C49B48DDADB245F3E37178707A7418A4A94EA5578281D4CA185DD2577B3EBDEB584BAEFDB59EF7D130FC887F09FC88DFE693653BFDE4C33A5DFF7177C5640ABC778539C2B073AD4D258B83CBB98F1CED9C9C43BBBB1CA2FAF6C66EA3E4CBF112F6173FDE283AB799A78C4B14125428FF14316449DBD3EAED27B144629F168B7724611AAF115D78975F9CBAB79538951E95B65B066DFAED049C1AF51B0D0F2FB7B82295B89BE1EA375BFD39A74FCF1EBF8B32F367BCAEC7CA09B8ED9791FA6DED3AB7278E7C15BEF574BA7577D0C71B47B9419FB5EDC4ECAABE1758FA509DFAF18CA5DD1F6C9716C2097E6C20D0C7609D3F343C9035EE483EF5D5FB84AD32D5ABA707F28209DBE7575DC9B3498B15DC05433AB72926DEA81EEB155317CBA60EA7878C6D2D19D5782B5C0F1D74A347F4EED62D38C55289FC7EB208CAEE317C797E85AB2C463E0632DCD39C1FA5DF2E6C5B950CF89E039D8AE32724138340E9FBF76F44CBE4A4F16ED6B336B7FE9697B19D9F6F28882F50D226EFA4458C3B29BAE33CF85644B6A03C5DCF30BA88EE98B91CB704544C9DD298C650EB35DA7C19029E27613B6DC74FF2B9A16AB5C895C5D0DC4AF2C95A15855B1C3B29C051596753510CBB254866555C514CB9365118A3058C9679AADC7E04917C3883275EC3155CE3C505588AF9403F86AF65813805A3897158518E7E52A7C8B4AE69A258A1E8327815C0A9E7861547EE3255055602A7654C19773A082B0CB6C194CA42EA196292D478C1F538DC1902A8571A4AB9862C9E942624C81AA0CB65C0D1863BE9A25D69FBF2AB12555602C3F7F9562878B3B1D58E87D759F8E2E64645DAD1984D25D613C64419239795272113931F5942B9199F9CED7A2855748E7D7F993863F460D5FA8D6CF29DD9FD6E9DB65A0424F5530DE5653C17E6A71DA6025ACEC44622D66F74AB80E63AC2F373DC7EF1F0D0C213E724019746FFDDCD681FF31F540F22D2D31EAB8370E9D5B68DA6DC6B6DBE8D892B4371D48AE83BB922E76D761F40DC68B94CC097C0AA3E62BB701B68A4CB7BEBC7BB414DA37F2615295181AB5CA601AB52B98D2A86C2BB369B4FB000C1A40B1144D5B5346F914F964916DF39771224CD97A0CA674318C2953C792A6B90C7E44EB0DF9A5262D5B1DA6305D4B4A68A6AA79B697605DCC95349F05576D5EA8A034FA7C0D50C704AA992EB793CDE664B1402BA22BC5028B570E98ABC8109C298749CD56B26415E2A6A9E690B216CC1879A1941F8A1A9D34632206F74933B60844FCFE2F3FEF681283334CA2977880C0D693DE3636BDAD547D6C342356C2004A93BD9F5CB55BED938CB1F04DEF1CC2FE7EB9C278BE4E2BFD4FB1D28D0C5B948A65B3CE2A53FD91BA6AE30963BA305DDC070C18686C5A0623DCF0DC1E5F204D5B72CAD1BE02175F98F23DF057A6A23A1AE85A5E9BEAA73FE5BB54E63FD56CA231BAC132A0427CA19B00D4B4AD16530E9D0294C7493F03A6695B0D82F8CC810931DD3EA9B6E7618AD5F5B7BB64D939EEEFB4638E6CC7A478175CC81077CF9966CDD255D7E616AB461377CB937B68577F3F52E8D412813A2D7610DB94F0F337F4F89AC4DB97FA943A39A1EC8BEC80D784401B902C1F81B6426B04E6ED01ADC002484717137860763248AAC34C326892417F421904AF09500649978FE080D0964136ED39196405A4930C120DCC4E06D137A1A38D2A2935AD2BE5D46D9EA155CEFDBFC66BB4095E90EB24A77A3E778580B4B97370D6F9D56288F7A7F99345CC925E6E398D30207F893842DF43F4C7D0685CFC408B6D3600334C3BE2B8AF8282A77D520C5DBCFD18C619645A2763D31CC56F15642F10B93325FB34D16A9952E12E8030B8B2C4ACECA1FCE16AA858B0B8EB4102C052D40102C14ED4D3A61E10486F67A8373B77433910F8E99547E08F7116AC06997620186F99E154EE7854C61BF59D41C5E4905B3E5A0162E9B553812B2D6C654D6021D880C99F058DF5907CFC61101FD729EFC28E6A53DAABF13489BFD9C5DC2A5A3EA0DFB72453AACD2AE021D81E4A74AC40BF24F176334420AC21FA746E6ED27A199F6E7BA7ED245DC62C5D4EB6CB307B4C8270B54FA68DFA60D9AF07B96552EACE1DBB7F1CA3F7649B24B62B14DCE959F324555A52A5BEFF3A794AB324586425B43D1231E51031A2B75BF2DCBAF7592F11C80F45867BFA4FA393375A0795D26C721B77B35963EEA90058C5C3BE4AEF9ED278859A1BA2C903621FA4D8499AC68B30376073A6072A7C1D3D8C8B687950BB2300D5E78C32C25A3F6EB6AB2CDCACC20546E9D3E1BF716452C2AF1F8537F0A9607B7407C75C07A5DA9285C1EA2C8E88BC0EA38C17CC61B40837C14A0B17A63528D8A154396472EA9ED89273B4411191CE5AE4D641818BBFCC635377CA6C412AA27D3C6AB1930D9761A819DE380D99AD6CE59BE7AA6E46C47A0C4A0372203307C68C98B71E901FCB474D27CB6582D254C880743588E3AAA756FADCC6C004D8ABC28A01FAFEDDBB6356CADB2C40F2A23AAC731E6B2C0AAA81976547F700900426F331CB781FEFA27344B49683C2CB85043748170194AF0CE3E1769D8263D05918E0A359AB050A4E940E0A5443189F3E16657ECE9EB7B849B886D89AE0D22C8FED064B93030BB0A298D9876747E1007AE043E19CE8F4DDB41A98FBAAC4F3E0A526CB297465310F5677E1C6ACC87400094690C9C7C28930FEBD31233C3F3ADD172D06E3C53CE8C95CE648D1700A5017E2C4223F9C01074270850C28E0F0E1F950328A6E7C0056AFAC37A5B5AC03034B667508BCFBE3F9247C09A340434115D477C9FB1C6C03CDD4C9315081470F825441652D5E641B0F2C56CFEE4E154C856B386423020D601C82C648C5650BE3DE58AC45732DBBC6DDE9B06CD4CEE32D9D7F30A937C354C4ABCE90ADA054E02DB04D8E719511A113AB0058F46295125156A7F3AACD60FCC3453F164DB438147233D14558228F7244820EC0776C420A15575BB19E882E3D082B110D74BA164648EB83E95A011C45F30B45736C66368FF16A20A5A0A8D972B665A099AEA876AC6C290B8381B38D5093C3D4DBECDD8CB60ABCAD831C1785DBCD98D9C0DDA6D671D3913381BCA5288AA27A771CB9201078FB8A28E5EF863A19F02521C1752649141FDC0D0B08428AB36A4E1DC6D529430061FA6438CB62F6D1D4A043621A924616D9BCC7DDDB7A7F15E3DFD32E2B9EA7D1EFB5C2888CFA0C23366638E6CCDDBB73538D6110FEB4308B0C6B0BD188D226622293906D0D3B81410F0D58D720EA9BA857E71BB24ED852FDA5A888616A2002C053BE79D72A7AEACDA2114D2DA27E8947D92504589BDAB2487E46C2B743383125429E995B119DD64415D20B55EB93DD35A3DD3A620233371DF39053A271778B3F45793C48C2C89979FCD8C7B25222E4790128A211EB339F6E68629F0B4033BAB12326303DE6B22998A44750613EA68E475B510AA7B6A31E1DDBCC8B81A748EEA4730867323DB939D9D3C9A1342E18BA8D9A4DF92A435198FFD5DE1A2DEF40D3CC338E139200F99E0E4782B9D1E9BD0AD934C8A92847F6325C112765991D99A907F11EC41A328E6341FAB622B7FA531991F9AA8E07DCA309B9D56BC9A43AC32EAB3A1E76055553B0741CF4C9B2D05DF12A51B2365DD7D9B019B0BDB078D3A7169B73D53D0CBE6F766F7A263FB5099057F630FC02AE77A6AFC3B209F7782E465B4B73089E8CD40536AADBF8750406E33E140386DEBBA10DE8BC8A642B3A5B34F66F20DD39FA8810D199BECE8E3E22CA6AF1CE289E3EE623C0E35D07C99BE21A88AFEA8C9168A8DA1B8F632E02B1E841F28809ABD339D5705861748E9E033C41C4A5568797B8EACEF889873C144F0931E98BAF8444D641806B3C0AFEFAFC558BAF3E7F75CD4F18A2B5338D0B2E6AFAEF997B1A521A70CDE7AFC3FA403FBC2294CDAFD2749B87B4104D305057E8099D573376866680EF8C122E41BE07FE93CC8B56EF659B31F020F5504A7CC015B6F0C28F4C17A68FE73ADD22566F70CD7C12540DCD1EFF2B6EA5949DF5EE7ED0C2E86C9B66F1DA9A7A92E6BE6828EB72484A5A3A0E6843F045CF31FB08D073DE9DC23A70FCF2ED2E50DBD203401B822F0A8FF6B29FE382EE14D681E397934742ED22FE5B1E69298C5052DBE32212D6EF1E6DE234CC62E0F9046985BBE666283D3C6842CA0144E30844036AD9E2204894215301AABAC6E1A154F606058032BE1204A10EBDA4C22157DC401CCA801C5A006A855504A7560D15E072AD130253BE595634CF2F04F9C6F9659CA2692BB303D7BEF111D1413F579A8563281571159BA502FE4A35188B765C80C0B0FE241A004560948DC9FB39A871F10E4F87A0CD0B21014D9BF7381A03A11CC844A362BCD4146061F59A870CEBAA4A9CA55A1184BF54A15074A7D8BA40E12993FA2A19C6B8C801A28C714E53912B78028942EE41D5436F652F916F1445285F05C022250304A84C14A1A44E1DF11DA44C130F5E05A89C15080A98FF945673201251F78907ADCA1CA9C4378F220504BA81AC47C46CE69CC54609B1D25C5A10E95D9DB591D194B0A6521525549B586058518D11B281459D918E0D25EA8F824C5C4B8064B2C897D45004B12F5BB8D78A9884208268972D288D36E6927FE8209772D69104C4144D2E1C12B31BC3C04130D5F4B661132ED222C429F2708CF4340B0332B6F1AF946619BF084330EA11D79A184CE03F213D64010281910842047254694E024AE2088202AA096D411B28101D401A65BC3ADE442E8858D71A4575A691104416A34E9BC4F664E162958949230F6B060D4A18D8CC8A44C250665EE44B13684B48112E0A1780753B0E97CDA8DB91B75AED8BD3AE9B41B6C34089C62A0C15C5A30C058BB21839141CAA05A67566EF4C052E9811400579C0230A7D61C8A316FAE5B15A420461A422060A85776752B442EC00441005E0A1100742F0B4502E4D0292810341775484B39DF376B81DD19C0B43F2F0B30505E5319F73280C8FFB950F05E0519000F43D17E2CFBA9E5B1382F534F722EDA1A83C2272C882F7F0A31084EF31278720604FFB2096BA398149E2F428584416D54738C182B83ED60C2388E4C3EE41AD4139A11810C347402D55B41F6E6492783F0C95580BA7826492083F9E642E1853468B4E4AA5531981C625ADFA504135E29B0094338D8A420DD4202E4A6BC80223B884A00691505AFD34A64C0FBC2818840E6B6AF8D828D849EE66E39271E5DE35A6936A45798B0029E034740DB4C250AA43A8156A82E47736D2C9EA105CA5AF65A21A9FDE96A7EFEDA3DCB3B41C7EDC6E935A7E3ED63C616735368FB9025B943BC66E612DC3F6D15B6863A9F462526E9CB68FD7D2D79A528D4F674D99F81D29185CD3F5C8E59AD2F438B2E609DB531077472D38FF48C3BDF0271551C017F3338F28C44BFB768A1D83D3036211DC45712E0422C0088F6F740C18EB53201DF5C5A3E5908DF7222085342C0C370A5160187DE39F12A44703021B82042089344A0985BB284E890A6F29107F86352026897CF86AB39A2476892D117A31AA01714AE4A400A3998886C0C633B125051BC1C4D77A6062978828210B71C20F4110E4C49C1682B0269ED6071FD0444D0ECD75220E7ED28528FDAD1736D0899A30404014D950E890285D88420741F16188AEC29F407B2A181A85DEF5D8E028EDDD33F715946D996C38145F7241E14C270FE0C14F8FC2814E7392FB759A03E24B8828218942C18F028E43614E0D38F2843F19C08748109143114C811F8A389C823959C40114BC93E6F3571549D8F7FF42F45B1100AC49D07AF3EF414C400FD5458E1AB2F7ECBC9385E0453B7B55563D6B50F96C08DEB0FB2509EB1925258CEC91B5683C8267D6DD88247858EDD3E34BF9A45AEE626A7AC9A1FD10BB9BCBE9E0F7199A2FAEE5C4D57EAB2D2283CE6BED6E84D6799CDD2FB9CDAF222CDF748B48627EF96041F7715E37183DDFD6E17D17F361F406DCC56A18F7DC985F2B58BE141711C9FC22C1622646787560FA285C677DB8980FA397E52ED6C7807343E21B11A8F5F3F1BAECE3D1C3E215AD83F2C3C7235C658136D93658DDC44BB44AAB829B60B321BD362DCB2F070F9B6081C77BF6EF0F87073FD6AB28FD74F89A659BBF1F1DA539E8F4DD3A5C24711A3F67EF16F1FA2858C6471FDEBFFFDBD1F1F1D1BA8071B4A0268D7DEC5EF794C549F08298D2222AE56598A4D97990054F418A27EC6CB906AA693E96AFFAE3DFCCF3D35DBD22ACDA90FFAB784AE4FFBBE77FF912ACC2E5367D57F6FF8E60D9E2E97F05A23395D02EF170D728CAF291239009F9C6B8F9C32258054915E2A9155DEA2C5E6DD79138DA94B8F56398AD100DA0FCA40FE31CA58B242C1FE4B4215105FAF0AEA2749BA025F9C10CAD5D600C8F2354F3591F56F1B2B57847D006D6FE6E0A6D96A2ED328EDED610C856A129DC07F4FB164545AC421E70BBD47CFCE55B4F880465913ECCDB38BAC6CA421C89880B56B086CFF31558411F3E17C0B60D5B19DD56136E4E5505F8561DFD5EA8F78C2CAB7085065221C6FB43C80A97E6AB3EA46B142C89E1B80DA7FA66CAB9789F81B836FF6C46B3C2C8715BBCF4616946155ACD3800982F3591F36B9466C17AC3CAFAFAB33E2C3A4C00CB327CA989A4C61B6A46DE25D082BAFA6A806388751D7E476A7DD687759770EC577ED2877191660FE14B345B642F3424AA401FDE5982F28723CCF85A9FF561618D307C0E7960EDEFC6989DBE81989D1ACD6285010BACFD9D87F6F188D1B358F5EE88D3EF4C144085DE7771DB49F14BA1F6DD74BF61E931FBD28D1E50FB61E9519537470AAD030775A1D899C092EB520DFA4A5B0B5736A77D186B62BCEA65A86B4D72CF9FDCB3E46BB1D38A214BC3B7BB1ADC2C6AE8E7C8DC9D895D1CBA2FE33863B592EADBB49C767839D581763AAF2741F0208D05256CE9674595DD5D87113AA6E1D02556103F08217E30E2474C1B8615F32F0647920C732EFEF63D8C16E81EBDF08713A8823EFCFF0E37B3181FE656FC3ECD14198C3ADE46DC79ACFEA80F67F61A470C4EE527038917FC60C41DF96070B84E56CC793AE122CB496931C9CAB1C9CA2A82923BFB7D01D1C6760F479A1213BA9BC86C0E4862FB4BFF370167F19AD02665E545F575187BD5B470C7652B2956CAE595F5224BA1B67ED659EB25551B88F081950B9A76128612BFBF6E32B1046C2F1A0511E7C4B42F9AB1F3D77CD59FC5B2EBDB2D79064383638A76555E5EA5173FF0EF903464F89D2A3181781D2FBE2176F5D45F27593EC9724636FC7AD35538A410881D950F0F5767DB2441D1829944AAC008DEC99A9CBC3868D5E71172C4D949678E80404C1CB1B31C61751D447384C98DD09F8E2366E71C24F2C908C6E91507837C3282F11867C18A03537E1D8DCE5C466A75A72BE7006D74E422034C4FA79A71AF8AB31873FE2A77D3788CBF2156D5E34A4D74DA87ED13F1497CC2139B72F72740B109D61BAC995D26F13A9F4A96B840B191B5A740AB4CCBB9600E1E50B9C1191745B378C522DC7C3531D12EC25598671F654DB5AD021B2B17B18D6F536ED44CA9813F0EB1604340A902030377BC0A176F45DA14CACADDFA6EE271C4C401E7962A54C18057EF4E39FE2C3E199C51EE4E98C309F9607045B3581025E5371430AB902A30F11D230EEE9819CEF35449B403195564C0253F3661F2C6036C7F37F41A1481E40A0D765BB4788D425C5286CFBD41D96BCC4CAFA88E7D2FA761B4841690AC9E7D6FB304ADC3ED9AB8C99DAC59855751D7AAD71C84B09FB2D460C5816A9E8D8E771DAEC3ECECEC37C61FB6FE6AC2DD0B94A61CA8D66703EF5A3C8CD775907C0329C7971AAE9A2508962932A4217476A20A4C290901A44B8C2D83A061D0EC6DC66245368A6D82758320E55F6870C556B06143265861309FE54243120085CA27ABE4AE5A2573C17073617B0C4BA1A69E5C5F72C97097DCCDAE192DA85D6001EF762680470A4C2465841EC27F22564C565F4734DF56D6E762BE4DACCEFB3DDF5FF06E85568FF1A6502840AD4554C7B61768DF846B8C88DBAC2CDB05B79958B4F79BDB7648BA58B92B14F3DDBFB70267CB697DFF33738F0CD2638C4FA9D19207561798D819EF5118A5C49F13BA90670A47C4E556B7330597BB7FA723B4B59D836680D6671358E2B31B5B66701ABE2A51390FDE983311536402538C295BB64B6B68305EB77AA857F0BAFB377ABB2A830B5D083CFD3345A3B9F313C41C3664212031A606FB80ADFC70CE6D902441167EE7DE2AD59F27FBC618EC1B965CDC78A076666551D6530D7E1637151B36A747A44ACCFE7CECDC0E2FEA667BCF81D9EEF082C67E24B50B4FE25FB6218343F1C5E0CE390F4A965383B96B6E17989C8148885CE032B6F5DD14DAE91B47E9D6F74900ECB0001045F8370D8B0084CBD758F470333FCBFD3C5E0761741DBF709771ED027D78BEC24F5111E1D921738526D7864C4875163658C118FEE7AF02B8458189E83959F07A74F375123B3B2C76D80C539D05100DD04214A90078D24170AF2C84EA9B8128C2629485527D33B8ABCF8224E31587D667133F3F4005A93F1A0B4366722091C856311134C55D1A672FAE3F4FA266C7458D230163295676E940536ED69C9769FBBB3134414C4AB6D018AEC8FB1728B674177D4B4B5812AFD1561D1B176BD7C11C2711343A11741D46608E52431144C0588820B8991F11D435D88B93D7DD989B5E622E7A4EFD755A4A3BBC9472C9FB88D61B9289D795CDB282676BB614B7F775C7D4D5147FBF5CCD82EC957157A93E4E2B648757089030DD89F24BC1B4D4841530FC9E92054FBA6CDE714D5C3F36AED7CD6F66C8F810580BDED703E3C9EE1DA678737A03A2A9D32513FBEF30FB97ACE59AE32D585DD8B23377D799CD0E30E25B76CDD340CA8F2413CE589CDFE0F46CE3A6B000E7B1931ACEF4356E520B701E11A9E511384ADC6C6270F8A332D8BAECEE1E3D43E133F85263C8BC41922AE8D71A52850389D9873EF5677D58FF40CC5E987F30B969B97B221CC3FBA634DF27356487D510E34CAB4EF6D02E67522378BBA2A05FA50F59127E438FAF49BC7D7965171B5338ADB81D5E71C6B95B9DA852F62BCE10DEB4E2A61537B21577B2D99C2C1668859200F3B68B7C2114409BB4210A007E16D1AFF11A6D8217C45DF95105C68B92BFDBA00A8CE15D2D387FCF7681C1132EE27788490E5F2FF2A5E690C9DF389A25E87B88FE80C133556CFBB8F881165B560B17549984D50E0BABC7E0C985DA1D3CD928D7502B3FB2C88D03E494A9C604B39DE07FDA91CA8D3EDC02D829C5260CC0CF0271E25733B1F7D8D8BBC8A2ED80AD0B4016EC2C6A28226F51FF01FDBE25E17259A6E64B0D9EE2E1C3D5867F89D3FA6CC05023782D3B4BB78CA33BF9302DD71D5EAE27DB65983D2641B872716CAC81D91C19258DFDEC400FF136E1E26797DF0CFCD6E040DC56F1B7DDB8CB93F0951B3EE33D55302DDA5D58B427691A2FC23CC2BD20D9F2BC4AD0AA9752B9A9AE973919CB8025E8B356019A3F06C90BF8BA5E6BC15650A0854B8853776E8C57B9902DF132C6A7B93CBF4A6FB7ABD5A7C3E76095B2E60CE1683F1E8153ADCF0DC4A37E5EBC2BD2F1D9AFAA428EF9D0FB2080DE0D988EB4CE013960801642DD9892C03044A8F3FCE57DDE936C1668998725D2F17FA51B40DEAD40942180722CAC8E136A413F3DB4BA4D2B0631E4AC16F2C96C62EB3662CF65C3E92D218E70862BCCBA4D72B90B0C31CF65CE819345B60D88B55D679ED9363A6F354574A46175A42303CCD55C3360C7B8492B47EE542A308F874C8403DB14E21DE95B20C582A4E18F506230087663780A58DFF20372C69F33BEC416CF025810366F0080395077D5712E68580EB84603E3318A22151D9C4822EA01D41C7EB462FC184B04C711076AF66738A9A2375D744FE2575E266FBC78B8E2575F1622528738DDD6A81F8ED727B72D3C11991DAC2589E3E65CF46CA18B77A91068279F5290A3CCB1E87C0607813A591E168319E3EEA04D220FDB84C2ABDA7CBF500174CFD266184C5B891695A63DC5E9C293BA26CF45EFB3BAF84F0B8176F29A0658CB068B8EEC2500EA609D580D668C7B8A36893CEC298A7703E67B8A0AA06B9636C560DA53B4A834ED29EEAD8FEC8B012DBB23D708B238CA3DFF45163D06F4988C8D2C6A1D2FBC296843DE4791840D6606E7B20534EBA2EC0F0A0B6E0E714C934DE16536D3BC835E459439ECA9E7719E89A3F9FC325C9158DF3A97C95C7D282ABADE5D3203AAE3E4E6D01C4C2E8BD5CEDD24B706A07B910C3581E655FB1E990738BED9DDE55BE4D630CAB0D126735C3781E618CA6A202764096E7C335C21D66D864BC48EFB9FE09365A10E062B7DD1CCB61188E763DD29A6E18D698E19CC76534E37833092D54033B1BCB698EB110A6D1EB99D15DCCD508A043686535E3612096F8BE9CE218E73B20BD47650823729C1E677B9277D19B4483DDD9296E2BC63544D0D0E1076D2910D5A701D3083184B338E281A7322A00D72DED4D135885561A880405C3468A6628F5CD83CD8B4F4D7500370E4A8A1EAA8235FB6C0BBB1E2CB911DA325524A02979C76B64DB378DD99DFA460DC739DA43B97BC67B40B75C179F42CC851C2250F76743B3080E4DADF40BBEBB14AC41DBCF8EF5934BA634E3D781E59540781918BCF5DE757AF72B4E355BB01240F6E237A5D8F558EEEE06577CF72D41D73EAC1F3C8A23A088C5C8EEE3ABF5AC8D1EA553EEE2C0BC208256C95FAD97FF9A5FE9D561F084F052FE8265EA255DAB47B58BCA275901323DD048BF2D6F8324C5292CE34780A525454393CC014F81E2E518287F39666685D32F7EFABB355884806D0AAC24D1085CF28C533F40D459F0E3FBC7FFFF3E1C1C92A0C52DC14AD9E0F0F7EAC5711FEF19A659BBF1F1DA57907E9BB75B848E2347ECEDE2DE2F551B08C8F70D3BF1D1D1F1FA1E5FA284D97541C8E5630108E53E8D9FFF80FC44D5B359DF7E8F94034F31F8FD8861F01EE21087C3A0C0901F275FB0B8A882B055ACE822C4349446AA11CD5C303C220C1D30AD54C7224055F46DA287A88BE07C9E235480E0F6E821FD7287AC95E3F1D1EBF7F6F0C958ABA41C3FE9775F0E35FDB00B364AB847715A5DB042DC90FE7B896B0193A1B02290226152E0F52FC2C21CF52B45DC6D1DBDA0FF876A4A7EE44C00264914911FDE9BD2907DCC6D1351683716445698B1E4C794DA70B2E1BBDDBB9A4C0E793E0A31762C8C652BF71D59380FFF0D35FCD65479D87D62DDED728C883493B659A8257F0A657C15DA245B80E5664A7C2FF918D0283C57B13D10C70F107D30E9A6B83DBB87E2BDD519AB6F8C41DD0C7106FEF59B0DE54D092F88FEFE51D8E68D73A8BD79B2DFE603C977C5E5E279B0C56ABB32A888188A38D59E421CCB601B41376845BC646F7C3761769F610BE44B345F6E2AB8B562CADB203FC330B89D43503D48EA3D50D522B8896BE503341B12BE476682DA99EDA5AE2368A2AE4BBA956557DEC36CA6DD8623626CEF3C8799063651FA72367FA8729C7393A66D940BD8CE34CA1D24CCBA397E5C1436E87BD038F541DD65815CF703F4C10E568AEC3081D7B9A0502FB83F375920FD62DCC870CD3343772E1D3F83D7A51AA8DE65DFC77B899C558535F29B76B739DF42CDE464A0DDA1CE5D96B1C39D7022E831FAE41E6790A26613CB430D6D7550A0BE57E88D1E64CDCCD8AE74755C1477D122A78076D0DD30AF4BE02415F5FBD85287675052A4B9D57D50BB8E9CB7E753128ECF532BD4A2F7EE0DFE13ABFBA2B003E85E643BD4AAFE3C537B4EC02631219FB2232728BF89EECD9A392286771929097E0C4AA4EAED52B58DB28FC7D8BC27C8C78A6137331805513721FFC846E82B465B0B059C767F10633DB651217515E0D4ED7A0C654A0D54A5BE1F4D2F21E45B378A5B0519983BD0C16E12ACC59DB31E446872407D12DB70D48C06BCDDE05391DFA018D091D2E7239E11AF25D12BE8451E0F5D2F3ECEE5401D5C2767E7377E21AE4C962418EF8BFA1A093A8B9C26B2D177DE7244273C7AD0D2B1961F2E602522E52F4C0E9291D68F11A858B605586D7BE41D96B6C32CD5A8721A693D3305A6A2CB1EE1DCD12B40EB76B724778B296FB7B74EA2CEFC5DB2DE43621925FAEBC98A37F1DAEC3ECECEC37C7602F7E2C509ABA877B8A49F0BA0E926F9EA99D2FB0A5E74E72DA9FAC899CF2763D9ECF83DF3ECAA3989393D8395AACC816B64DB0AE13A4EE8DCAAD0E76D7D253EB835CAF4F440330D62FA763A0C76320179869B04B662BAF8604F36CF85DE52E610E79E2398F3C07077C1BD6A5E68305F3291C1C2CCE4813DBF9B678E53EA07B62F6F263BBFE651BD668CB6C567A87FEFC01503E70D76BE52A4DB768E9E2BC5A403A7DEB66409CD6AE4F474C2E06DDCEAEDAF3781D84D175FC62767C307FBAE0FC69C72CC127B3E4CDC67AA777F8790EB6AB8CD8093D77F1F96B2783F7557AB2686B9D5606F8495AF89316241C6F19EAF135DCEC89DCA88284DBEF5044887683F0900549E6C4A67B1139D9BA4B91C44C78A74BF2C294D9E9A67D5ADC9E17F7BE2C692F0A7CB9C9995D5B19EC9E162F0B0DA06B5E40DA1875A87BA9B7B4ECB0933CF4F1806E921E4E9C9DF954BC9A0A9FB6242249E3F74412295DAEDFFFE5E79188B7333CE297D8BD77FCB4EC7CDBDEE05CD23BBB66940660F3871FF7CB15C6EB7562ED1D626D2EFD920D7B0B3340F155C5C99D54EBA1FB09D29FE3D6C4A1FE38541D09776765F07998E22DE58D0AE23019EF87D2B9F99CF3D6365671923C87207D999999A087D618B3C1E85C1F60AA04ADFB210BCAD1601C69277E57B35AC2579B267E3277CFF27150AA5E34C45D04239E536797C157E9DD1389B5D9987D27E3EA50A2BA6466126150E118A33AF71B28CADA31A6775606B9D247AED2872C09BFA1C7D724DEBEBC4EEB65E8F50233EFA4E38C5EC73188C83C499D49EA8C4BEAC0CC3B499DD14B1D5926F3E1C2B8995F99FE1AAFD12678418621794CA496DA8C6E0FFB6AE1C1C92E77FE2221DE2D2EA28C3A207FE36896A0EF21FAC3732F173FD0629BB99F88691FF079F5103CED8906D3DD35CDCB0DF6C4BD1EB997769E19C91EA9B677D898EFA6276823E3BD22E0BC0DCFF1991E4C39D065AE885FF0416DE3C1D5DF476208433D4FCBC92BDDBA1EF8B4EAFCADBA93ED32CC1E93205CED89DA52A6DF72EC04A41546C91CAEF12D939ED7AEDB1445D3FAF3B8FE88C3AAADFB3C690BA73384BD99C0747FAAE55A7762BF23D67DEB823072F4B20C9720200858177761453BE381039C5E76ADBDA37625A77D9603138A16BD0C45D456EF9D6C2B4684EDEC6F6B425FAAB3A1C8CC23E145007437EC9A5096EE6D28D2025874B1321BD1BB4BAC10134A57FDCCED028C38A0328381D446E18AC2C4EC66B9A591A6BEB7B4BA8F1DD9D208BEF65B9A09453B6C690E88DAFB964670B60D016142D6B22E90F7B707AA369D3BDFB9CA757E3C2D74D70BDD3B49FFA42BDD3B5DF775A93751AE3A2785684069336BBBCBB9515209BA69C714130CE61DF85F302057102DA3CE5BDCB1A5B32F03195D9D8FE4E276A7479233677A7331C6410087BF3C3AEF5D7237BB7662E0ADE1DDCE9CC02339E91EC27FD627A66EF1830D27F1D79B69125DC0FB82D214AD1EE34D11F0DB4F3071BA139751A70DB9E6EC64E29A9D5FFA9757BB3189955949E9F7623D9B7F0AEE80E13FC60F591075CAA87595DEA3304A49C252B3045F86FC3A4EF58F6F3D3BF793096376DE47B681D3AB127B1260CAF1084EAFFA18C128978D21AF8FF380B07B2234A7A62811451FA92C2D15EC3F6F36CB872B4FCA33863C88C64CB3839DE63CB1C39EB2839D4E33B1830F76989DFBD21830F0D32B8FC01FE32C580DC2C73B7788B4C94625A2C9499AC68B30CF93CA9978E7AD40F60C852EA2E5C17D4CBA6AD729D17A40ABE777D4F79BED2A0B37AB708131C033C90DB401D7F44E416B7FA681FD1B07ACF4DCCDC260758667374B024C6C7E3EC368116E8215300CA62E38FB90830AA1720D972D39471B14110F677EA03AFD713905F8AEEB1E18A654D1E3E3518B096C780343CD82059BCF6562912159249F9171700A39BFD4E966854C5216B767B4FAC4B2063B9EBBE81C91784607C5235A129C365D04900319EE6E6C5C05914530C1A0B2E48D9DA86983BBEE818F4AD78793E53241692AE49EAABC3D75F5377ADEDEBF7B27932EDA7CE868FEC191F9610003562BB1823D0E7A99F75C359B8B78B6CBC2F52440EEA8E73705E3949FBC308EE172EECA3BC550747A6A101B987B688711F1CEA33B715E19A7F210E2D1A84BFCEC3FFA13EB8485404F284187121B411F5C941B5EE73207AECE93E889A572CC292CCA2F1E59C8CDC49A9B7F8C78B020C31028F6C7AF09C9EF1CB855B9C5FA52CFACD69BB66CC0292CC1071658FC539266B648597BAEF2DF7B2892381A0CCD2418A161D9A2F4C32D93AC8A6D35296BA44921EB8C3F16295203737C527EF663D8E1DEB478E41620F5B1A8B75642DC41F826F78FF76403569CC2B5D87057ED7ADAFC360EA36FCE0654D6D9EE1AC588A6BF3F95427BDEB914BFC3CD3B970E789A7BBF730FE65F1E7CFE3F7FEDA63168CC3969DB9BC8EF69C7AFC76530F3455AEC41663C7F3FDF64F0159BDBF3F035EDB92A3EF4A225C259866B2CDA455E78870FDDE3877764E9B3053D0AF397F5C53C0DBE73E0A1F8384E1943F34F5FB2C7827FAAF8AFC3091F3A5FACD73D474B86EDE09CEB6A986C5EDEC1269D0F833FDA7D87CE30C922C294EEFCEE23C9A729E874F00D08CA7F305A97937170547F67203B9E1AF81404A5ED9C531F85FC5595B767B5FE66B45781B943DB60E10A5EF8051CB51F8651A74C15F4CBA7C01C89448246243F667510103BC4546EB71B6339E29F23C5F97EB49097E5F619DABFD38CA78DDDB576888BFB75C9B2E645267DD3D0FC73B64DB3783D71D12E731147DBDE76584936D3395CDA83BE264FB24A6FE2D29ABBAEC119649B15202049F939129D4E3AC6A194BB7173E0AEA87B2ED8776FF53E4BBE77BD758F9BD3FBDDCCBBF3EB2875C389D3F69DD386D21FA579A9E770690FFAA33C5D36C3CCB29ABBAE3F1AE40D17B3992879F348F447E91887D21FC7CD81BBA23F7667DF3DD61F2DF9DEF5AE3E6E4EEF775777226EC7A73F4E9CB6EF9C3698FD31781AB1A75BF044EB04E4F76EFB37B1A9D605DD0CEAC896A74F241B6F3A2F33938A2688D4A466A8F8D00B73D45928D9FE3DBAC1E5C3EB814DE00C9B82BEC429357B6696C72079619F868DC4F76D0866E9CBD3CD8859C4C9EC7A7391BD4724B7E2F2ECEE54295F867697645F1BD7DF769A61C00CB4BBC12F0A1133DCEBF0FEB9A5A717E226CC02E76D1B80570A1BC1E8C50BE0BFDBFEBCF342C6C033773C72A6E21D85A8198FEF75BFFC3384B7B5819BF5387888CA223D7A314467F96611614A775E2849729A0B3A1D8F6C62F84A21A2C4F32A9DD3BDE634F3D91F84E5F834F4C3F11E9DB77DF4D28C4697C3842DDE7979C60C6847041ACB540A51269954F984EE39A3594CFE302C473719C76E4AD2118C5E9C553913381C9A829D1761F55076447851FCA3905CE0FC89E66E0FF9C8686E3B472633E6A6AAF27CE8F86497E18A4449D3308A0F79234BBA62CD9CF5B79DBE99AD46A1D3553E61C386B56AB1CB686DE2FD334B4F3671135E19D426DE62154D93F8D0C2053069B63FEFBC883130688E46CAEC88457C28F6E93706E30E59C45B2C54C6F172247DC41E8AA43E08644F6231EA47DE1D8BF0A8665E213CA699773CF3433994E6DD9F2C8B575518DC6E1C688E2125F5780FD8261FC68E080C866BC67EAEE99567FA3DD9E8B1CCF0479B866376EA78C3734EFD7DE7054E35929D93393B77CAE99B8B8638E7E8F1D1280E3A0D27919FFD1D75783628BFEEBC2829C6B17382A498FEBECE3BD3F40F7EE469A5A7A232092A658071BE28EF79ABC459208575FCB96919649A7295D7CA3C13643B37D908F2A3310CA85266C697F473546CB86B89412D7918CA36AC1F67826A3C8634A2CDC3EAD99739E19D06E0FC3289D7ADA4DCF3C7B8F52B9D7D71F7965EF190BED5250C2F2FD1789EBF1B6FE5451436EAB057DEB9B835E39D8BDBDE79077729E01D52B277BCC352786CBC938BE09B0B906D8ACCB69863F27FD29B0BB952A8CE33AB4C694CFAE0A0E41F7D31867E325917FA1A47C351B2C3AF373AECF0EB8D7F76C07DF0EC403EEE093BB0341C253B9C9DE8B0C3D9897F76C07DF0EC403EEE093BB0341C253B5C5EE9B0C3E5957F76C07DF0EC403EEE093BB0341C2B3B54FEC8C7C2293772285704F4329AFB9D7408379AFD0AB561B9407068658482E4A8EA4C283067D3E6E39E0885B11F46732C05E750861D24A74F67ECC01C379B8F7BC20E633F5F529636C1C182B6C661F6A00DC2928386B52D58CC365CDF42A85E0F223B64C595CF564FD8F6CDCA8243918295258724EFACCC1C9EF8C28995798AFD195859A0BB295859A2CB79676546C7E30B2756E629B6D7AC2C304A14850DF3CACC12450D80B1CCF9943D9DB6BEFAE5CC3EF44E01258D3A73CE1717B84DF686DB64B8054A5A27F1CB3049B3F3200B9E8294F76A21AD1E50C65DA41D1E1465829BAF87C52B5A079F0E974F319EE8E089B9F4011884EEE8331E7CF247126664D9703D51A55057AD0AEABE2A4F4FAE9BAA00EAA1742C55023F592E1394A600F4BA04025F166A74502D411EFBB200C4BE60514DE0F57624E8A32E1777558959658FE52996EBA9FC0EF5509CF39490F3A7041CDCFC2B0495BC5650C26C2C741CE0A608825E956A12A4F41B83A952160A4993976B2CB9145E6BA97091A53AAB8B046F289F8BBD861BA003B602D4155D47AF534157E20ED4608B40E81CD8E233049694684E7113220D9EE5A65C38D1750C392DFA307978406231754494A3AAA97B87B3D67208C0D5201CE02CBD2A34EA24585CCF7509D45959A8436469BA3580E0D2FA30F1A529E854182A127A403BBCACBE60D3972539516E9F4CBC2D7E17652A809B291D964C3D712423053F3DE42B3809C19316A94BE58780946A4F4505B90645EAA83B3D4DE26FA050AF0AA04E8A328DA9D962A67BC40AE80A9A965621382575B99ED41508F4A648247DF5047B1DD41B9482421D210F4CD404323710BB5279ABEC4C57F904E26F82DD6A6C30ADDE2DF61AE532862A89D0305ECE543C2CB06BB9B2D61A7B1E1D4C4B9582D9A92E1129544D0820AD5E84DCD42E94F7A5CB4CCDB37BB033B99E48850DD0A5DFB19880C7E2AEE807C7065414755797AA7B34A5A5A8CBB24CDDA19E0E2EF4AF971C28348E7682CA269B616E8C94EC8579B9C29840AAE87748AE36651D92724587A48A0EC573BF4C98C47991F8304B4AB57A20376C821E4891B80752AAD503B9F810F4408AC43D9052AD1E88D54FD0032912F7404AB57A0099AC2912F7A0C15ACD85B9A007114335A526461878C6F92A6A538C0607F057608A9E451CC1D732EA199C3FBE8ABA678DF96C99A2055D0AB9B255CC74D232CA4242694E19345B9539F144D564ADC6AD77A4BCF914F7CB7CE7ACCEF0DB8656F3F667F609183D44EBE1E716EA45A64F85AAC1DE1083C8057C36AE9437291DE8BAE221D06A628E7EF569E08197AFFC6B63383F5CA686185DC6D49EE35A7F930C539B3836C32BEED7DA240346C855723827E0155E7BA8E5274743A5DF1B8A47CBD47380B2EC869D6B5C97741E76E1B2C740E5470D55738E3E0583BA49C9DB965F5C0D39095FC22890CB2A5155D7A2CAF760F333BB687840182736A4550BB1FCF77043295FB5A7E9162D4FDF848362AA4976D794DD5653C57ECADF6835A32C3F771E6A1E3C42A1557175FCA9105A44B21DE62C09D741F2265E84402DD7EBCFEB00CFD173B05D65448A2806C9D7DCC5817EFEAA1A20AEE17245920AA40F1F03CB732B5077B7FCD0B83A6254DB1701399AC507C9E0E00BE8BA6DBBC8C9601B80450E76C178D96A2EE7B3EF2113E1B2C09BFBC922DB6219239C62A69AEB212BF9C27278D4F5F6BC000C8F10A8E99A95C16BFBBA3953EA61F862092CACEB5A06F74F06C8C1604E7D8408A2D14A3C48C63B211F5AFD4D421C9973450E04AEE08153045E193A8C0337F5C611A32060CBACA34D39651B5F86841110E96C9B66F1DA8654B296FB4630A987D01C2E85D7A8051CF7D24DCBA7AA5CF4D29A5E1403B933969ECA2085D18F041C0F91DB71998CA9ABDFD8D7A21F25210BE9D7899C5A20FE144415FB36CEE1520165CDE1B897AD5ADEA0D54CC96A7A90AD2A37521DD9AA80D18F6C1D0F915BEBD99CBAFA8D7D89815112B2108C9DC8A90562FF89FA183C89CD675599530352E3F45D2C6BF2BBF330880B726E07AAC34603E3E12B89D16C3F41C9F12C3E4806C6BA4FD7AD1C59C85AD8978189E54384A21777B48BF91E623B3F703BF791C8CE0954756D06646F15EB6FEE072B9E5551557797A1FD0D95C935A318AD2C338D3323AF40CDF03470BD69962554716ADAED67F0F45B06CDC9871BB9E601F0E146DD9C29F549143DC6801B29BC00C0E14987360632310F50545C23A8EE9A5FE03736757BB6D80F2914BC22A82E1E986450F2018D833CED57439AC285AAEB9A47D8675075CBA6C0C3C0F5040855573C0C700822F40721C0E7944EE52E9E77414DD7AE1DAC02557F733E54F14C0B6ABAD3147B1BA85A511457763DB380AAD4FEEC63D85A53EC594BEC79E84C9E6BF9D06549B11DCC38D8CAA50F1A98E0596FC8CAD91EE990E1C4C6A2316BA4417623B38F2159C647C3723368C5446B64F17521BDFB1AB2A60CD7CB44EB4C8EF3A3AFBFFB2280F6B4F726D17B2442FE785A9F07A8EA3E841C3FF4F2AB9F816BCF3D557DB7062E4EA2279E76651B8551C3E46983A0B5F8FD8FB08E4752891945D94622275C3D711A8C609D32D2492F01BB66B8737B23C8876A600040CFAABB90D322499B949C5D93BEF9216713328025271013C072396BE72D13AD6DBBC467FC2255BF67E3DBB4C26734CDA0D01896C4D1CEE225228E5D1A3047C469E240B48803C474B0248E764E2B1171EC926239224E13AAA2451C20EC842571B4333C8988639722CA11719AD0162DE200412BEC89D3E43B9210409014C991B1B797A16A27F51191C12E2B90233E60F67361C41B4BE268A7B81111C72E478E23E230BBB330584FC73809D6095F548115DC649271AEB18B6219F160DCEC656E5292A888ED26D7896762337BA232245347625B27CD5011DB4D360ECFC46664AB320A952DB10DD23A08E96A9B1A02183E30680DA2B1DBB4388E96984C1F8F0A00758683BAECE35111ABABFC807F667112BCA01BAC42ACD2FCEBC7A3FB2D6EBD46C5AF7394862F0D888F18668416A4CF066855E72A7A8EABA40E0C465595AAB87A4B8AB2601964C1499285CFC122C3C50B840F7EC46DF64BB0DAE22A17EB27B4BC8AEEB6D9669BE121A3F5D3EAAD4D0C922042D6FFC7230EE78F25555D0C01A319E221A0BBE8741BAE9635DE97C12A6536571108A2C0FD8222E26E41E612335E865EDE6A48B771A409A8245F9D30A37641BA8B1E82EF488C9B9A8634C53E9E87C14B12ACD31246D31EFFC4ECB75CFFF8CFFF0F1BDFBE8DC4A60300, '5.0.0.net45')