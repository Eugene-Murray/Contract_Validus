﻿CREATE TABLE [dbo].[TeamUnderwriterSettings] (
    [TeamId] [int] NOT NULL,
    [UnderwriterCode] [nvarchar](10) NOT NULL,
    [Signature] [nvarchar](256),
    [CreatedOn] [datetime],
    [ModifiedOn] [datetime],
    [CreatedBy] [nvarchar](256),
    [ModifiedBy] [nvarchar](256),
    CONSTRAINT [PK_dbo.TeamUnderwriterSettings] PRIMARY KEY ([TeamId], [UnderwriterCode])
)
CREATE INDEX [IX_TeamId] ON [dbo].[TeamUnderwriterSettings]([TeamId])
CREATE INDEX [IX_UnderwriterCode] ON [dbo].[TeamUnderwriterSettings]([UnderwriterCode])
ALTER TABLE [dbo].[Quotes] ADD [Description] [nvarchar](256)
ALTER TABLE [dbo].[Users] ADD [NonLondonBroker] [nvarchar](max)
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.Submissions')
AND col_name(parent_object_id, parent_column_id) = 'Description';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[Submissions] DROP CONSTRAINT ' + @var0)
ALTER TABLE [dbo].[Submissions] DROP COLUMN [Description]
ALTER TABLE [dbo].[TeamUnderwriterSettings] ADD CONSTRAINT [FK_dbo.TeamUnderwriterSettings_dbo.Teams_TeamId] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TeamUnderwriterSettings] ADD CONSTRAINT [FK_dbo.TeamUnderwriterSettings_dbo.Underwriters_UnderwriterCode] FOREIGN KEY ([UnderwriterCode]) REFERENCES [dbo].[Underwriters] ([Code]) ON DELETE CASCADE
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201310181100113_R3_201310_8', 0x1F8B0800000000000400ED7DD972E43892E0FB9AED3FC8F4343366934AE574B555B765CD98CE2A59EB8895949956FD12463120899B116414C9C84EF5AFEDC37ED2FCC200BC09384E023C22F9240501381C0E87C3E170B8FFF7FFFBFF1FFFEBFB667DF00DC5491085BF1C1EBF7B7F7880423F5A05E1CB2F87BBF4F9DF7F3EFCAFFFFCDFFFEBE3C56AF3FDE07359EF3F483DDC324C7E397C4DD3ED5F8F8E12FF156DBCE4DD26F0E328899ED3777EB439F256D1D187F7EFFF72747C7C843088430CEBE0E0E3FD2E4C830DCA7EE09F6751E8A36DBAF3D637D10AAD93E23B2E79C8A01EDC7A1B946C3D1FFD72F8D95B07AB5DF20EB749A2357A77EEA5DEE1C1C93AF0302E0F68FD7C78B0FDD35F3F25E8218DA3F0E561EBA581B77E7CDB225CFEECAD135460FED7ED9F54917FFF81207FE48561946270516834F8C36A587860179800E91B412B1B1C467DF7B4091242DE663D5CF36FE8ADF5017F5AC4D116C5E9DB3D7A2E5A5FAD0E0F8EDAED8EE88655B3461B8200FE2F4CFFE3C3E1C1ED6EBDF69ED6A8A21326E4431AC5E85714A2D84BD16AE1A5298A43D2166503607AA5FA780CD2352ABBC13382D9EAF0E032F88E56D7287C495FABAE6EBCEFE597E3F798B93E8501E642DC288D7708404DDCED5598EC62B4223F06EB5C4E5E3198D338FA8AE2338C87F610BA8E20EF7A91A0DD2A0ADF3603F5FF80FED86159846C11324C3D3FD51CCB4FA2B1E4BFC59DDF46E175146232BA994E0314AC2F0A151C3E852B14FF230ED28138BAD57FC60883A0F17F76518A7BBA7B7E0E9A7CADD8FF879FFEDC1581F3086F51818148EEDAF135F2F00CF4CDFA39C77B2FD578CF911F6CBCF5E1C122C6FF154ACFCF87070FBE47607ED0ED804C287A784528BDC5FF249AE3C3FFDA5B57C320F088D5B824F536DBB2E7D320F462AC17109D6E17C75880BFDD64D03354A418FDACA5869C459BED2E45ACF24361592B570449ED85D79D4C5721D659534217BD15DF75013C0458ABCE35D67E3BBE8B1BCBDDF6AABB48D287E0255CF8E98BAB2ECE624438EDAE221C3E6720C2ECBA8030F307CF810D48054AA7DA4C24DC3674C6D047D7B7DEB7E0256359BEA83B3CB847EBAC4EF21A6CF363DFBB7A8D2F5B552FE368731FAD5B52A05963F910ED629FEC1091B0DAA317BFA0D408DF4AF75446BB6A21C3BEA8A83888B2B6EE585A7A8B6C1854657004AD3A22E4DB1575F1BEDB66688218E765CBC6B9BB812A5358755DE2C8D62847A18ADC8D177F45E9972826C69607949271C2A8363AE13402890CD615111B6EA04BF433BCD3451BA3D1099B826314B4108D54D44C77BC8F28DE24B77869AD0252AA3964596B70D4E246A2814B5A9ACD75370AA8C110CCBE393594DAEBD204C3FFBFC84F1FA3B3B5B74B90263564AD413A881B8928206969C60FDD28A00643C00FE6D4506A0FD1E4E3516DD0159A799BAA89819D37B75AE85A7A07B17518D899BA6BAAB3DA3E84DAAECCFD854ED8FB05477F0636EB5CAFD4ABC9358B856E2FA328D536EACD8B7C9A8B5CB4ED9FAC56314A38E7ACFCE85655691CB25A25CCBE4C1577DA76CBDEF7E86AB518D27510A2E3DEB9AED1F987FED77F469D9E3B7D48F12CE16FDF82D047F7E8C5C0A6DA1987BF07DB4594A4DEDA40A1EB6CD13D8B76A18105BBF3A017AF51D8BF2279494CFE3DF7F9295ECF7BE90FB197AA2BCC9995719FB6ADFAB4DDD5AF6320A5F72CDA6CF050E7ABD659A08C47A048EC91D50D47F77B10464BE7DE94E85DD214BEA5A2BB9AA2CAB2108A0C9AED72CE8D0D5509BAB4D194CE054413219D0360453558B9E8E776B77922876E5DE15EF7D545E85248CCF25BC30724B9F88E7F07A4F74A84479835BD50DFC734B98EFCAF68D515CEBCA7CC7B8AC99E528A605BB21ADE5538025DC76B01C11B4A56B46CCBEF068E4031B39D4075745D006AF741019A59F992DE252964A14A30CA60CD4EDB6006769FCE28A3DB2ACF222C8A73EE788CBEA24A7AFDBA0B56FADB10D6D5123F0E9ED08D9734ECF7A67BC859B4C5E288F063C60802B229D9D94AE4309B20B2FFE8AA089D5DF6EF51B888D6DA97479DFBBDF4FC601D64EBA3EFAE6BED9D983977BABA91B07F251EBA20A6C581FAC6931DF8993CEBBDEBBB3878C10AD7B06F02CEEE4EB5BBEDEE1E707377D27B9F27BE4F8CD8BF23AFA33CBECADE2BE2E542D4C7AEAA243E1504F19B0D4899FC5503A7763A40FE2B26B8B75EC4818FA7E806A5AF91555651B27B5158E023CACA485458C76411A34DB0DB103FF4938DEED3329BD864683873852FCE817D8FEF3AD804E9D9D9EF7DF77BF1DD47493240C7A798CAAF1B2FFEEA78423331B172DC49367B271B226F9D3D03C926CA6D1F85B5A87763111EC79A6806BB182BC25E32C065730383F9CA43EDC8C2A08BB58B3FFF499FF004DAD6E4D5D67C1DBCA796B6CA4665C796459BDB44F62E6544E9B38C0859B62E83305D858734534FDB67FDEE54806A564A23873F72D021259D3CD5487F63F58FB5E1141E63191B7CD33D6A5B08CB318BB6517BBA64CBE7E6E2F0E0D44B508152B50A0F8583C935C0BBF86E71DDBB8652F57DBBE8BD6FE27DF910FC13D9D17DF566EAB79B79A6D4FBFE8CCF2968FD186DF333ED4007EA361636CF4D7ABC737632F3CE3456F9E595C94CDD07C957E2A1ACAF5F7CB0354F338F5836E684E8317A48BDB0B3C7C955728F823021DEF4468E305C35BEE43ABE2E7F79B5AC2B512A7DA30CD6EC9B153A29F8150A065A7E7FCF3F452BD1D543B8EEF769B38E3F7E1D7FF1D9644F599C0F74CBB238EFC3CC7C7A550CEFDC7BEBFD5AEBF4AA8F218E768FD263DF8BDB5979D5BC6A32BC3E702B863237B87D725A1BC89D3A774183DDD1D47C60B2601BD9E07BD717AE92648756365C2F7248A76F5D9D06670D666C1730E5CCCA1C74EB7AA06B6E590C9F2EA83A0E9ED07474251660CD713A3612CD9F12B3B8386315CAE7D1C60BC2EBE8C5F25DAE922C7118745965F55271A77BD73D1631D62FE337278E956AF7F8CFDE6E9D920BCAA171F8F4A5A357F65572E237AFED8C7DC5E7ED6D64DBDB23F23637883C51209B05BC77B4EB2C3321DDD8358062E6E9095447F7B5CC65B026A2ECEE14C63283D9AC5363481531BB195DAEBBFFE64DF3552E45AEAA06E257948A502CAB986159CC820CCBAA1A8865512AC2B2ACA28BE5C92A0FC3E8ADC5334DD7A3F06C17C3885275CC3195CE3C50958BAF9003D86AE65813804A381715B91867E5327CF34AFA9A2D0A1FBD278E5CF29E5861547C63255059A02B766481A733A09C90D374194CA42E61A65B5A0E1F3FAA1A8561AB14C6B15D45174B4617E2630A54A5B0656AC018B3D50CB1FEF4458A2DA90263F9E98B103B5CDCE9C0D4DE57F7E9E84446D6D59A4228DD15C643EAC5A995E73417A1155353B112A999EF7C2D9B7BA5748E4C306BF863D4F0B96AFDB2A5FBB775FA6619A8D0B72A686FAB09673F35386DD0125674223116B37B255C87B92C28363DCB6F3F350C212EF25F69746FFCD4D882FF73EB71E85B5260D4716F1C3AAFD2BCDB8C6DB751B125296F3A905C07772555ECAE83F02B8C17295912F82D8CEAAFCC06D828D2DDFAB2EED18A6BDFC886D9AA44D1A85106D3A859419746455B914DA3D90760D0008A85689A9A328A67D8277EBACB5E05F230A5EB5198B68B614CA93A8634CD64F023DA6CC92F3969E9EA3085DBB58484A6AAEA67BAF136F95C09737930D596B90ADA469FAD01EA984035DDE576B2DD9EF83E5A135D29E258BC32C04C458AE054394C6ABA9221AB103751398714B560C6C80A85FC90D7E8A4191331B84F9AB14110E6F77FFA79A2091CCE30895EA201827ACF7ADBD8F4B642F531D18C680903284DE67E7AE56EB54F32C6C037BE73F8FEFBD51AE3F93AAFF41F62A56B19B65A2A96C93A2B4DF547F2AAB5278CEEC2B4711F306090B579198C70C3B37B7C81346DC12947F90A9C7F61CAF6C05E99F2EA28A06B786DAA9EFA95ED529AFB55B189C2E806CBFE0AF1856AF253DDB64A4C3974FA531627F5EC9FBA6D1508E232FF27C474FBA4DA9E070956D7DFEEE255E798C7F38E39B21DB3C5BBE04286B87B4935AB97AEBC36B358159AD85B9ECC43BFEAFB9144A71608D479B183D826849FBFA2C7D738DABD54A7D4D909655F6407BC2638DA8060F970B495B646A0DF1ED00A0C807474318107662683843ACC2C836619F403CA20784D803248B87C380784A60C3269CFC82023209D64106F606632A87D133ADAA89642D3BA544EDD66D969C5DCFF5BB4415BEF05D94EF0AAE673970B48933B076B9D5FF943BC7FCD9E2C62967472CBA98501F94BC411FA16A07F0C8DC6C577E4EFD2019861DE11C77D15E43DED936268E3EDC730CE20F33A199BE6C87FAB207A81C89C29E9A78946CBB4156E0308C32B4A4A4B1FCA1FAE868A458BBB1E24006D8B3A4020DA997ACAD40302F94D867A8B733B9403819F5E3904FE18A5DE7A906907820117D95DC58E4745BC53D7D963740EB9C5A31520965F330DBAD4C256D40416820998EC59D0580FC9C71F06F1719DF33E4C549B525E8D45702803BECF5B3EA03F76244BACC92A6021981E4A54AC40BFC6D16E3B4420AE41827FD9363729BD8C4F76BDD376962E63962E27BB55903EC65EB0DE27D34675B0ECD783DC302177E78EED3F8E517BB26D9A866F7ED6BCE75285B80C366236F5E339CF44F31CC281DE614851B58003C14B9E92747EC7F2432C49B14B91C0819F5D9EA0173FA71AE824CCABABEB202D8B27C7E98B13614E5E5B7530B22874CAE2B1720F38794AD2D8F3D36266F748032B868811BDDD916814BDAFC00281CC66A429847F1A9D3AA624FA0BABF26DD46D07C3DC5302304A577095DC3D257819D517E8B383D89476149E143B4992C80F3259CC58665BB2B73D8C8B707550796B01D597D4598D360EDFECD669B05D073E46E997C37F63C824855FC5CCA8E1B7768A7607C74C07C5A92E0DBCF519DE86B0BC0EC29415CC41E8075B6FAD840BD51A14EC900A4B26A7EA892E39475B1412E9AC446E1514240A35C1A6EA94DA826444FB78D46027132EC35053BC716A325BD1CA35CF95DD8C88F5289406E4406A0EB419316B3D203F166F3E4F56AB1825099701DBD5208E2B5FA2AA731B051360AF122B0AE8FB77EF8E69296FB20049C089A04A49AFB0285A0D9C2CBB760F004960321FD38CF7F12E3C47446B39C89D0049EC97C4F7203B02C6C3EE3A05C7A0B23040CB88D10205274A05855643189F3E166566865C36B889BB86E89AE0D22CAC9A1A4B93010BB0229FD9876747EE007AE043EE9CA8F45DB71A98FB8AF3D712F4F9A039A55D99CF83A5AB90362B521D40821164F2B170228C7F6FCC08CF8F4AF7798BC178318B09B514F999D59C02D48538314FDFA9C181105C2E0372387C783E148CA21B1F80D54BEB4D612DEBC0C082591D02EFFE783E0E5E82D053505039F56DF23E035B4333B5720C94E0D183209550598917E9C6038BD5B3BB530953E11A16D98840031887A0315271D9C0B837166BD05CC9AE71773A2C1B5D5E2D4B9F5F3137352A72998A381D6BB255132CC05D156E522342275601B0E8C52AC5A3AC4AE7659BC1F887090ECF9B687EA4F87AA2F3FB5E877244800EC07774BE1E19571BB11E8F2E3D082B1E0D54BAE6BAC1F4C1748DF8B6BCF98582DDD6339B85C0D6905250520131DB52D07457543395809085C1BC025AA88961AA6DF676465BE62550418E49526067CC745E035DEBB8EEC8A93C07421479490F3A8E9C9327A1794594B077439D0CF8828C092A93C44B9F6087053819176835A78A726D95218028A6229C45214DDBD468470CD6248D28F1438FBBB7F1FECAC7BFA75D963F4FA3DF6BB9016BD519866FCCB0CC99D3BB73938D6110FE34308B0C6B0B510862C963229D8896353B813161355857232826AF57EB1BB24A5467F5A52809F1AC2102C053BE7ED7327AAACDA2164D0D8222F247D925426293DAA240A75AC2B743B44529428E995B12BC5B4715528BE4ED92DD1583815B62023D371DFD887CBC71770BCFD7F2781044D9D4F3F8310FF52745C8F10290046B57673ED5C8ED2E178062F0774B4CA07BCCA533D4098FA0DC74751D8FB6BC0C774D47BD76E84727069E3CF79DCA219C4A8467E764DFCE9DA770C1D06DD474466C118ADCF4D8E6D66871078A669E719C9038C8F77438E2CC8D4AEF6544BB414E4519B297C19A38298BECC8543D88F720D610711C0DD2B515B9D19FCC88CC56B53CE01E4DC88D5E0B2655197651D5F2B04BA88A82A5E3A04F56B9EE8A578994B5DB75AD0D9B02DB0B8BD77D2AB13953DDC1E0FB66F7BA67F2539900596507C3CFE13A67FA2A6A25778F674258363407EF494B5DA0835E8E5F47A030EE4331A0E83D0D6D40E555245DD1DAA2317F0369CFD1878788CAF47576F4E15156897746F1F4311B011EEFC68BDF24D7406C556B8CD486AABCF158E622108B1E240F9FB02A9DB71A0E2B8CCED1B3872788B8D4AAF01253DD1A3FB19087E2292E267DF11597C82A08308D47C15F9FBE28F1D5A72FB6F909433476A6B1C14575FF3D734F4D4A0DAEF9F465581FE8875784D2E55592ECB29016BC0906EA723DA1B36ADACED014F0C928E102E47BE03FC1BC28F55EB419030FB61E4AF10FB8DC164EF891EA42F7F15CA75BC4F20DAE9E4F82ACA1DEE37FC9AD94B4B3DEDD0F1A189DED9234DA18534FD0DC150D455D0E494943C7016508AEE839661F81F69C77A7B00A1CB77C3B056A1B7A0028437045E1D15EF6335CD09DC22A70DC72F268A92D0A18CAB56BAB440F6DDFB703218335EFDD65214827E13F2F19441FE676F9DCA96031B827BD244AAC262749ACF4CE38B84B34C391F2F3507702EA333BD65B823C386716062F08515C1126243157EFD1364A823402DEB6915678B8CCF6991C1ED4F13E811D8D61D636A0060920482DDE94802AEFD85928A5315802A0087E0741A8E2E2C970C84ED5200E45B424250095358107A73AB74BC0652601084C115042D23CF3D6601B679E1292A68DAC644CFBDA814F05FDCCA2C11D43612591B159C2E1AF4481B1DA5E651018DAD94F01200F8CB43179DC0C35CE1F49AB10B47EBEC9A169FD585261202DEF5EDEA82817620958D8F6C142860D09529C854756087FE1694FD29DE45C010A4F914A2E936194FF3220CA28CF6119B9BC279028C449453EF446E63DF146413E4A01E6E9C420404592332975AA6C452065EA5C462A8CCFEA013CEE87B43A4907C5B44310AB37071488C6860FCD413BBF41A33233177C1D9777FC8434DB6A4494B6C0E8385288A5CEDA80D8561B68D5B34D09632A9531A295890506955618211D56DA1AE9E840D2EE284845350648268A7BDC1A0A27F27103F74AD313108413EBB801A556F76CF24F3BC4B1987504E19079930B0744EEC63070086439BD4DD88489B30B718A38186F7B9AB9E1789BF8975AB9885FB80178D5886B4C0C2AEC2B971EA2F0B0C04838016219AAD4470D2971382161E58436A00D148614208D345A297B41CA8957DA1845796812104414A15499C4E664612255F249230E6A090D8A1BD6D28844DC40964EE44B1D66914B112606238075330AA3C9A89B71171BEDF3E3B49D41368300F2C6CA0D14C8A20C850A341839141AB001A66114E84C0526941D400571B83BC6760A06BC6BA05F9CDB0544E0C6A9A3A0B4F0EE4C8A4680358008BCF06B2DC481006C0D940B9B8360E040C83519E14CE7BC196C8D37E7DC806CEC6C4121D9F4E71C0AC2667FE543E1D72424005F1E71F1A71F1E1913827E67E444DA4331D978E410856E6347C109DEA64F0E4EB8B6E6412CB1730213446993B08828A61B77823951DD8C198613C78DDE831A83B2423120821B875AB2586FCCC804D1DE282AD1265409C904F1DD1CC95C30A298129DA44AA734FE984D5AF5A1822A44B70228A71B13AB35508DA8588D2173ACEC02826AC4C16AF4539B321DF02267102AACA9E061296127B193A54DC615FB56EA4EAA11E50DC26381D3D035CC1645A90E81B65A1324BE14124E5687D05A7D2D13D9F8D4B63C755F4FE99EA5E4EE69779B54F2F234E60933ABB17EC42DD8A2DC3172176D19368FDDD536960A6F3EC5C669F3685D7DAD29D9F854D6948ED7A984C1151D4F6DAE29457F53639E303D053197E09CF38F30D8177B52E185FBD23FF3F0027C356FA7E831583D20E6A1BD24E74220FE17F7F8D68E00667C0A6CC7FC726839A4A37D7148210C0AC68C8217164CDDF82705E9D0804007A00248228C51D5C29D17A54A86B710883BC31A10914A3C7CB9594D10B9CA9408BD18D5802855625280B1AC7843A0A3599992828E5FE56A3D5091AB78941005B86287C00971A54F0B4E502B47EB830D67252787E23AE187BEEA4294FED60B1DE64A4E18201C966828ED80585D88D20E81E5C2105D06BF82F6543030567BD7A343633577CFCC1951B465D2C1B05CC90589339D387C133B3D12073AC549EED7690E882EC4A3842006113B0A380A913E35E0B843EE64001B2087470E49281D7628FC603AFA64E187CF714E9A4F5F6424A1A3BF70D16FC47F31264123E28B0331018529E1396A88A299B04E169C7826F45559F96E42E6B3C18960E29624B467949030A2101BBCF170826C74231227AC864B8F2F69400DB18BA9EE258772188E6E2EA783DF6728C6DB1013573952078F0C2AB13ABA115A253447BFE4D6BF8A308CE8C12389FEE58301DDC779DDA015BC4385F76DCC875604101BAB61DC73A37FAD601827844724FD8B04839918E1D5816E481095F561633EB4E28AD8581F239C1B5100118EE95E29DE08636F97451CA14CF9E01B4089495F1663C4910F972496853A15D5CC1A1A3130DCD0D48D3184C4B52030AA000A55D9C7A307FF156DBCE2C3C7235CC547DB74E7AD6FA2155A2765C18DB7DD9241D52D8B2F070F5BCFC7033AFBF787C383EF9B7598FC72F89AA6DBBF1E1D2519E8E4DD26F0E328899ED3777EB439F256D1D187F7EFFF72747C7CB4C9611CF92DA942877BA87A4AA3D87B4154691E20E3328893F4DC4BBD272FC1B373B6DA00D514C34594FDB15123D8F92C9FB9966DC8FF65B847F2FFDDF3BF7CF6D6C16A97BC2BFA7F47B06C08DD7F05A26E14D02EF17037284CB39123504AB28D71F307DF5B7B7119BAA3111DE72C5AEF36213F5A0EBFF56390AE511B40F1491DC65598EC62B4223F28549A05DAF09881D59FD561E56FB1F387294D60CDEFBAD01609DAADA2F06D03816C14EAC27D407FEC5098873E6601374BF5C75F3C1E86485014A9C3BC8DC26BAC7D46218FB8600563F82C5F8115D4E133916E9AB0A5617014E1665495806FD451EFA5F540966615A6501DEE7984E579400B83FAAB3AA46BE4ADC826DE84537ED3E55CBC2F405C9B7DD6A3596E35BBCD9F8ED1346B151ACD3800982DD591CB1B94A4DE664BCBE6EAB33AAC76600B9A65D8521D498D37C0943C74690BEAF2AB068E01D64D88864321577F5687751733EC577C52877191A40FC14BB8F0D39736A456813ABCB318652F91A8F1353EABC3C21A5CF01CB0C09ADFB5313B7D03313BD59AC512031A58F33B0BEDE311A517D1EAD811A38FE9286C123DEDE2B693A29640EDBBE96AC3D263F1B91B3DA0F6C3D2A32CAF8F004A0784D6A1AC338105075505FA0A5B735736A37D686B62ACEAA5A96BCD72CF9DDC33E46BBE1794264BC3EE020ADCCC6BE8E688DB9D896D1C922FA328A5B592F2DBBC9C26BC9CAAC84D9DD713271A95C282E2B674B3A28AEEAE83101DB7E1B44B8C207EE042FCA0C58F9836142B665F348E2429E65CFCED5B10FAE81EBDB08713A8823AFCBF07DB45840F736B769FA68A34461DED42E63C567D5487B3788D420AA7E29386C4F3BE53E28E7CD0385CC76BEA3C1D33B11085B49865E5D864651992CB9EBD3D8768626B874397F109DD4D64D60724BEFDA57FCBFD59B421B4496879517E1DC65E352FDC71D94AF295727965BCC812A8AD9B75D6789AD704C27DB16783A69D84A1C091B49B4C2C009B8B464E08433EEDF366F4FCD55FD567B1E8FA7647DE55B5C1514553959757C9C577FC3B200D297E6F95E840BC8EFCAF885E3DD5D75996CFB29C920DBFDD74150E090462A2F2E1E1EA6C17C728F4A9496C1568C13BD990931703ADFC3C428E383BE9CC111088992326CB1146D7416D8ED0B911FAE1386271CE40229FB4609C5E3130C8272D188F51EAAD1930C5D7D1E8CC45E85F7BBA7206D04447CE7316F574AA19F7AA388B30E7AF33378DC7E82BA2553DA65447A77DD83D257E1C3CE1894D98FB13A05807EB2DD6CC2EE368934D254D5CA058CBDA93A3556446F3A9830754AE71C645E1225AD308D75F754CB47EB00EB22C70B4A9B6516062E522B6F15DC28C9A2AD5F0C721166C0868AB40C3C01DAD03FF2D4FF4D3B27237BEEB781C5181E599A50A55D0E0D5BB53863FF34F1A6794BB13EA70423E685CD1F83E51527E471EB50A5B053ABE63C4211D33C37996DCABED40D62AD2E092EFDB207E630136BF6B7A0DF24032851ABB2DF25FC3009714F1986F50FA1A51D3CBAB63DECB6910AEA00524AA67DEDB22469B60B7216E72271B5AE195D435EA3503C1EDA728D55871A09A67A2E35D079B203D3BFB9DF287ADBEEA70B78F928401D5F8ACE15D8B87F1BAF1E2AF20E5D852CD55B302C152459A3484CE4EAD025D4A4200DB25DA9641D030A8E5858DFC35D9287631D60DBC84BE72068A8D60C3864CB0C2603ECBB986C4010A95EB508234DEB2FEC6AD82D9CA39552B6726686E2E4C8F7509D4D4912B4D2669EEE2BBC535A555350B0CE0DD2E38F048818EE40DD143F04F448BDDF2EB88E6DBC89A9DCFB78E157BBFE7FB33DEFDD0FA31DAE60A0AA805F1EA98F602EDC3708D11719B91A53CE7361D0BF97E73DB84A48B91FB433EDFFD7B3F30B6A1C6F71F997B44901E237CEA0D572CB0AA40C76E798F823021FEA1D0053F5538222E37BAEDC9B9DCFEBB1FAEEDEE1C342B343EEBC0E29F05E9328DD3F55581CAB9F7469DB1A8221D987C4CE9B229ADA1C178DDE8E15FCEEBF6DFFC4D5506E7BA10684DA08A467387C8098AADC94240E65605F6015BB9E19C5B2F8EBD34F8C6BC7DAA3ECFF68D31D8370CB9B8F668EDCCCABCB4BC0AFCCC6FCA3794CE8F52A598FD78ECDC8C7F6B677BCF8099EEF09CC66E24B50DCFE45F77018543FE45E30E3B8B9A975183BABB6E16E89C81480C67E072B7F15D17DAE91B43E9C6F759004C5800F05250E8865900F239282C7AB8999BE57E1E6DBC20BC8E5E98CBA766813A3C57E1ACA8285BD4964B176A1CB99BA910685232853A977A542E011A3658411BFEA72F1CB879818E483BF159FDBCFE3A8BB3098B333AB55A67C1D6066820E264001CE936B8571A42F94D43C461F14C4329BF69F814A45E9CB20A49E3B38E3F22A0DA541FB5852135399048A4ABE8089AFC8E8EB143579F67513371516349C0188A95291D948ACD9AF1866D7ED786C6899D49176AC3E5792903C5866EAD6F49014BE0DDDAA863E20A6E3BE8E42C82462782AE83104CCEAB2982081803110437732382BA06A5B1F20A1D73D34BC444F9A9BECE4B69C24B2993BC8F68B32529A86DD9424B78A6E6507E7B5777575D4DFCF7ABF5C24B5F293798F2E3BC4226BC4288C699DB6F04A96B0C94DF164C434D5802C3ED2999F3F4CCE4BDD9CCF563E37AD5C47E9A8C0F8135E07D35308EECE9418237A73720EA7BBB6466FF09B37FC15AB639DE80D5B92D3B737795D2EF0023BEA3D77C1B48F191BC571A8B531D9C9770DC14E6E03C7652C329EEC64D6A0ECE2322B5385248819B49AC107754065B17DDDDA36728CC075BAA0D993548B60AFAB58694614B22FA0151F5591DD6DF10B517661F746E5AEE9E08C7B03E2FF5F7590D99B01AA29D62D8CA1EDAE54CAA056F2A0AFA55F290C6C157F4F81A47BB97577AB15185F38A9BF08AD34E5A6C4595325F719AF0E61537AFB891ADB893EDF6C4F7D11AC51EE66D1B794D5A004DD29B4800B85944BF451BB4F55E1073E5D72AD05E94ECDD46AB401BDE95CFF891360B349E8611BF434C72F87A912DD5874CFE46E12246DF02F40F183C55C5B48F8BEFC8DFD15A38A7CA2CAC262CAC1EBD271B6AB7F764A25C43ADDCC8223B0E9073461D1DCC26C1FF6D472A3BFA700360A754A03000370BC48A5FCDCCDE6363EFFC758905B62E9EA9E8B333AF218FBC79FD07F4C78E84F5A5999A2DD578E2870F575BF6854FE3B306438DE015EE22D9518EEEE4C3BC5C27BC5C4F76AB207D8CBD606DE3D858013339320A1ABBD9811EA25DCCC4F92EBE69F8ADC101C38DE284DB71979F2365EEF9A2258E668D67A2769DEA58C0869E752A80B80BC1CE233447EF6C1F82973CE62E253BEACFF3F29AC2F23A4992C80FB244179C9CEBCB324FB35A66F5BABA5A0275BC4257A04B680968F9E8C52F60500CA5FDB084022D1B429CAA736DBC8A7DD2102F6D7C6ADF94ABE476B75EFF72F8ECAD13DA5AC81DEDC72370AAD5B9813C5859E6CFF6549EC49455A1772FD0F33B80DE35988EB4CE005960800642DD9892C0D044A8F3FC657DDE93A4366895451353D909DB0DA02D0E080E06508E86D571420DE8A78656B769C520869CD55C3EE94D6CD586FF3040737A0B88239CE112B36E935CEC0243CC73917AE4C44F771EB9CC529967BA8DCA53681E1DDBB03AD29102666BAE29B063DCA4A523B72A15A8B7793AC2816E0AF18EF0A99D6441B6E18F506250087663F816B0BEE507F4D66549B9EA1BBCBAA141983CB101E640DE55C7B968C3B2C0350A188F5114C9E8604512B5DE172EE13761DA6F1D79702C71A0627F9A93CAB3E9B47BE25B79749E50B270F98F2A0D44A40A71BAAD51371CAF4E6E53783C325B584B02BFE825EF555017E76D2ED04E2EDB2047E963D1F90C0E02B5B23C0C0633C6DD4199440EB609C9A305FDFD4206D03E4BEB61306F254A549AF714AB0B4FE8F9BFE43D7FECF23C810BB4D3A30480B54CB0E8C85E1CA016D689D160C6B8A72893C8C19E227996A3BFA7C800DA66695D0CE63D45894AF39E62DFFA483FC851B23B328D208BA3F8610DCFA247811E93B19146ADE385770BDA90F75124CF8A9EC1B96801CD3A2F698BC4829B411CD364B7F0D29B69D681A724CA1276DF7138CFE41DC7F2325813D72195CB64A63E94CC40ED2E9902D571723368162697C66A7237C98D01A85E24434DA07955BE4766018E6F76A77C8BDC184611955D678EAB26D01C43C948C4842CC08D6F864BC4BACD7081D871FF137CB2CAD5416FAD2E9AE9361CF17CAC3AC56D78639A630AB369CAE97A105AB21A68C697D706733D42A1CD223759C15D0F25CF3BA539E545239EF03698EE0CE238273B476D8212BCCEE4B7BCCB1EAA1431C1E4D32D68C94F17D8AAA9C001DC4E3AB24103AE0566E063A9C7117963460434412EEB3AAA06B132CA1B10E7AE0D9AAAD82317D6EFA10DFD35E4002C396AC83AEAC8970DF076ACF86264C768891492C026A79DED9234DA74E6372118FB5C27E8CE26EF69ED425D701E3D0B3294B0C9831DDD0E3420D9F63750EE7AAC12718217FF3D8B467BCCA906CF218BAA203072F139757E752A473B5EB56B4072E036A2D6F558E5E8042FBB7B96A3F698530D9E4316554160E47274EAFC6A2047CB57F9B8B3D40B4214D355AA67FFC597EA77527E203CE5BDA09B6885D649DDEEC17F451B2F2346B2F5FCE2D6F8328813922DD87BF2129457393CC014F816AC508C87F396A4685330F71FEBB375804882DDB2C28D1706CF28C133F41585BF1C7E78FFFEE7C3839375E025B8295A3F1F1E7CDFAC43FCE3354DB77F3D3A4AB20E92779BC08FA3247A4EDFF9D1E6C85B4547B8E95F8E8E8F8FD06A739424AB56989B46A80E8653DAB3FFF16F8899B6723AEFD1F3016FE63F1ED10D3F02DC4310F8E5302004C8D6EDAF2824AE1468B5F0D214C521A98532540F0F0883784F6B5431C991107C11C826EF21FCE6C5FEAB171F1EDC78DFAF51F892BEFE7278FCFEBD36D4AB30D9C568457EB8824DD14513481E3F2C775110E267087991A0DD2A0ADF366EC037039F7527025EF07E2A44F4A716A269BC93C2BE8DC26B2CB6A2D088D2063DE8F29A4A174C301ABB73D9029F4D828B5E88E1194BE9DAB54E00FEC34F7FD6865FA765B68BF735F2B2D8EA569926E715BC49957057C80F36DE9AEC2CF83F22D83158BC97909D1C177FD0EDA036F3DF46D5DBE61AFD7FD978DFFFB5031BDA03FA18E0ED38F536DB125A1CFDE35B71E7C2DB65CEA2CD76873F68CF259BA6DAC208AE42AC06A765D0011E476BB3C84390EEBC3C1C9B55B845AA00376C7791A424CED5C24F5F5C75D1887D5574807FA60191BA7A809A71AFBA416A04BD52176A3A287685DC0C8525D42B1B4BDC44B1847C2DE5AAA58BDD46BA0D1BCCC6CC790E390F7284ECE334634DFFD0E5384BC72213A89751944A549A7979F4B23C58C8CD3075E091AAC31A2BE30FEE87C9A018CD7510A26347B340607FB0BE4EB2C1DA85F990629A6646297C1ABF472F52B551BF8BBF07DB458435F5B574BBD6D749CFA25D28D5A0F5515EBC46A1752DE0D2FB6E1B6496B66316C6430B63755D250F93BD1F62B43E1377B3E2B95155F0519F84F69DA0AD615E81CE5720E89BABB610F9AEA94065A1B3A97C01D77D99AF2E0A85BD5EA657C9C577FC3BD864576D39C0A7407FA857C975E47F45AB2E306691B12F2223B388EFC99E3D2A897216C53179B94DACEAE41ABC84B50B833F7628C8C688673AD6170358352149499ED08D97340C1626EBF82CDA6266BB8CA33C2AABC6E91AD49872B41A595CAC5E5ADEA37011AD25362A7DB0979E1FAC838CB52D43AE75487210DD31DB8000BCD2EC5D90D3A11BD098D0819FC909DB90EFE2E025083DA7979E6777A712A806B6F39BBB13DB204F7C9F1CF17F475E27517385D75A26FACE4944E58E5B1B563282F8CD06A44CA4A88153533A90FF1A06BEB72EC261DFA0F435D29966A5C310D5C96910AE149658F78E1631DA04BB0DB9233CD988FD3D3A7596F5E2EC16721713C92F565EF4D1BF0E36417A76F6BB65B017DF7D9424F6E19E6212BC6EBCF8AB636A670B6CE5B8938CF6271B22A79C5D8F67F3E0B68FE22866E524768EFC759067ADBA475E62DFA8DCE860BA969E4A1F647A7D221A80B67ED94AC5379B852773C26462340D767F6DE43011E3E5107C937962E8439E79CE21CFC1B1DF86F5D6F960C07C12DF0983E3D7CC76AE8D69997BE99E58D4DC98C57FDD0515DA2273989A3D217B0B940DDCF65AB94A921D5AD9380AE7904EDFBAD926E7B5EBD2C793094737D9557B1E6DBC20BC8E5EF43465FD5711AE5F8D5839F42C627C908CDF4C8C8D6AC792676FB74E8959D371179FBE74B2CF5F25277E539335BA2F9825903B0944A2FD1691245F83ED9EC8A23206B9F9AE47047337080FA917A7564CD017A11575A01049D48477BAD3CF2DAF9D1C03E6C5ED7871EFCB92767228283639BD5B368DDDD3E021A40674C5FB52134351EB1AED2D293AEC240F5DBCF79BA58715DF6C36D3AFA2C2A72C89484EFA3D9144520FF1F77FFA7924E2ED0C8FF825B2EFCC3F2F3BD7F63C3855F564D78CD4A8ACFF4EE57EB5C678BDCEAC3D21D666B23B99B03737C1145B959F3B4AB61EBA9F20DDF999CD1CEA8E43E58176272B83CF83046F296FAD9813F385C0503A379BD2DED8C6CACFC16711A42B33331553D118633AD69DED034C99FF753F6441311A8C63FBCD81AD592DE0CB4D133FE97B93B93828950F30A22E8211CFA9B50BE6ABE4EE8984F2ACCDBEB3717528515D303371059438DBC8CEFD1A8AB27208EBC9CA205BFAC855F290C6C157F4F81A47BB97D779BD0CBD5E60E69D759CD1EB381A019F67A9334B9D71491D987967A9337AA9234A943E5CD439FD2BD3DFA20DDA7A2F483382908ED4929BD1CD615FF90E1CF732E72F1241DEE0224AAB03F2370A1731FA16A07F38EEE5E23BF277A9FD8998F70197570FDED39E6830DD5DD39CDC60CFDCEB907BDBCE3323D923E5F60E13F3DDFCAC6D64BC5778C51BF01C9B984297036DA6B6F8151FD4B60E9E0FB87891A0A9E7293979253BDB039F579DBB5577B25B05E963EC05EB3D515B8AEC5E969D8094A23EE9C3D5BE65EA184B60767E1DDBFA23769AC6B3B37EFC9598776EFDBB2D39CE6F44329264214666BFBD092D06E2BD6DFA9684B4855387C24B054CAD29E3FBAA1373C6AFFA5605A1254A0CE391700802D6C55D18D14E7BE000A7175D779115064EA4AE299AF73214511BBD7732346A11B6B3F3B90E7D5B9D0D456616092702A0FB2D870E65DBBD0D455A008B2E572E5AF4EE128C4787D2653F4B7385AD2395290C84063B5B14263668C32D8D3475BDA5557D4C644B23F89A6F693A14EDB0A559206AEF5B1AC1D934C68A0E598BBA408EED1EA85A776E7DE72AD6F9F1BCD06D2F74E724FD4157BA73BAEEEB52AFC3C8754EE852835266D666974BAD8430EDA61DD3C3509877E07FCE806C4134CC186170E19C2C3E0F7403617D2417B7931E49C69CC9CDC51807011CFEB2C8DA77F1DDE2DACA6D4705EF7661051EC927F910FCB33A31758BFDAD3989BFDDCC936803DE67942468FD186DF360FD6E1201B43BB119315E936BCE4E66AE99FCD2BFBC9AC624966625A91398F16CFE10DC01C37F8C1E522FAC286B167DEF1E056142920DEB25E7D3E4D771AA7F6CEBC5B99B2C368BF33E32859C5E15D893686B9647707AD5C70846B96C34797D9C0784E989D08C9ABC24327DA4A13554B07FDC4CB40F578E94670C79108DB9CD0E669AF3CC0E7BCA0E663ACDCC0E2ED86171EE4A63C0C04FAF1C027F8C526F3D081F4FEE106992498E4793932489FC20CB71CC9878970DF7558A4217E1EAE03E225D35EB14683DA0F5F3BBD6F79BDD3A0DB6EBC0C718E09964065A83AB7B6F416B7E6E03FB370658E1C69E06DEFA0CCF6E1A7B98D8EC7C06A11F6CBD35300CAA2E38FB90830AA17205972E39475B1412777F76A02AFD499C9949D7550F1453CAE8F1F1A8C10426BC81A1A69E4F274C9A59644816C966641C9C42CE2F55AA682E9314C5CD192D3FD1AC418FE72E3C4724B8D741FEA29C446A4E7C0F7220C3DD8D8DAB20B270261854969CB1536BDAE0AE7BE0A3C2F5E164B58A519270B9A72C6F4E5DF5AD3D6FEFDFBD134917653EB434FFE0C8DC308006AB1558C11E07BDCC7BA69A2D793CDB65E13A122077ADB76839E3149F9C308EE672EECA3BF950547AAA111B987BDA0E23FC9D4775E29C324EE921C4A25195B8D97FD427D60A0B819E509C0E0536823EB82833BC2E450E5C9D27D1114B6598B7B028BE3864213B13AB6FFED1E2C19C0C43A0D81FBFC62437BB6757E5E6EB4B3DB35A6FDAB206A7D0041F5860B14F49EAD92265CDB9CA7EEFA1486268303493608486658BC20FB7C862CCB7D524B4912681AC33EE5824CFBDCDF049F1D98D618779D3E2905B80DCE2BCDE1A19A707E19BCC3FDE910D58720A5762C3A9DAF594F96D1C46DF8C0D5A2998BB6B14239AFEFE540AE57967F25D0F37EF4C6EEC79EEDDCE3D988C7CF0F9FFF4A59BC6A030E7A46D6F22BFA71DBF1A97C6CCE739E20799F1ECFD7C9DCE9A6F6ECFC2D734E72AFFD08B9608A7DCAEB0681639E11D36748F1BDE11E592E7F4C80D8ED517F3D4F82E8187E2E338650CCD3F7DC91E03FE2983210F277CDAC9939DEE394A326C8273AEAA61D249AA079B743627C468F79D76BA551A11AA74F2BB8F20B92CA7D3C137202819C8685D4EC6C151FD9D81CC786AE0531094C376D9FAC8E5AFB2BC39ABD537ADBD0A4CA4DB040B5770C22FE0A8DD308C3C7F30A75F361FEC4824123422F131AB8380981053D9DD6EB4E5887B8EE427BF52425E94E86A68FF4E3D9ED676D79A1017F7EB9265CC8B542EB3A1F9E76C97A4D166E6A229731143DBDE7658416ADF255CDA83BE26CE38DCDEC48535A7AEC169A45EE62020C87F3B129D4E38C6A194BB7173E054D43D1BECBBB77A9F21DFDBDEBAC7CDE9FD6EE6DDF97594BAE1CC69FBCE6943E98FC224ED4BB8B407FD519C3B9E626651CDA9EB8FE2E129B2192F93F948F447E11887D21FC7CD8153D11FBBB3EF1EEB8F867C6F7B571F37A7F7BBAB5B11B7E3D31F674EDB774E1BCCFEE83D8DD8D3CD7B6AEB04E4F7B4FD9BF00854BA19DC918D4D763A6EC72620392B8D0D5465F22E4EB2A4B49C9E07F77302F8CB59B0AE1F82E97A7F04D681FBC6F12C2CCB124B46912C8B6CE4BCB926355B339B7FE885CFAA64BB74FF0EBD7DB3E1F520C0E044C29CBEF899837B6696472F7EA15FC08E64271C8259FADAEDB498859FB3B3B79700F788A4905D9DDD9D4AE5CBD0CA131D54A1FA36698601136D4F835F242266B82018FD734B4F81307498054E4F3900AFE4A6D0D18B17E09942F3F3E4858CC60384F1C899927724A2663C4F4CFAE59F211E9568BC2619070F65F1601ED1664B7E8D5E0CB5B06510A14A272F94DAE399966CA2F84A22A2F8F32A9CD3BDE634FDD91F84E55A2D06E6BD93EDF6C4F73103C45E1AC5A397666D74194CE8E2C9CB336A4013116834534944996052C513BAE78C6630F9C3B05CBBC9387653927565F4E2AC4C0DC3E050174C5E8455439988F06AF18F447281F3C79BBB3DE423ADB9B572F7A6D5635979397418C6CB604D6EFD148CE2433A9E90AE683367F56DD20E28E52854BACA266CD8E87D0D7619AD4DBC7F66E9C926AEC32B83DAC41BACA268121F5AB80026CDE6E7C98B180D83E668A4CC442CE243B14FBFA1662764116FB05011AED092F4E13B6293FA20903D0939AB1E607C2CC2A39C7989F09867DEF2CC0FE5379F757FB2CA1F8F6270D338D01C434AEAF11EB04D368C89080C8A6BC67EAEE99567FA3DD9A8B1CCF0479B9A632675BC6139A7FA3E7981538E6472326772A79CBEB96888738E1A1F8DE2A0537312F9D9DF51876583E2EBE445493E8EC909927CFAFB3AEFCCD33FF891A79185AF9530552A03B4D3E2394FCFC74F76CBADE3CE4D4B23A19EADF47DFA096F9B2918479006926240993233BEDCC6A362C3A9E53F36E46128A9BA7A389D56E331644BAEE3472C3E2F09EFD400979771B45936431D468D5FC9E2B3BD90219278218D2E61785989421492698404E15158ABC35E79E7E2568F772E6E7BE71DDC25877748C9DEF10E4DE1B1F14E26826F2E40B6C91378638EC9FE496E2EC44AA13C9DB634733BE98381927D74C518EA39B36DE86B0C0D47C90EBFDDA8B0C36F37EED901F7C1B203F9B827EC40D37094EC7076A2C20E6727EED901F7C1B203F9B827EC40D37094EC7079A5C20E9757EED901F7C1B203F9B827EC40D370ACEC50FA231F73A75CCBA15C12B7506BEE27E910AE35FB256AC37201E7D04A0905C151D59A50A0CEA6F5C73D110A633F8C665872CEA1143B084E9FD6D8813A6ED61FF7841DC67EBE6C59DA38078BB6350EB347DB202C386818DB82F96CC3F4CD85EAF42032212BAE78B67AC2B66F56E61C8A24AC2C3824396765EAF0C416CEACCC52EC4760658EEE266165812EE79C95291D8F2D9C5999A5D85EB332C7289117D6CC2B324BE43500C6D2E753FA74DAF8EA9633FBD03B3994D4EACC3A5F5CE036E91B6E93E216286E9CC42F833849CFBDD47BF212D6AB85B47A4029739176789097716EBE1EFC57B4F17E395C3D4578A2BD27EAD20760907647AD08D74C4FAD52A8AB4605795FA5A727D34D5900F55038964A819FAC56314A12007A5502812F0A153A2897208B7D5100629FB3A822F06A3BE2F45195F3BB2AC5ACB4C7E214CBF4547C877AC8CF7952C8D95302066EF615824A5E2B4861D6163A06705D04412F4B150952F88DC154290AB9A4C9CA15965C02AFB584BBC81295D545823714CFC55E832DD0015D01EAAA5D47AD534E57FC0EE460F340E80CD8FC33049694284E711D220D9EE5BA9C3BD1550C3925FA50E9C64062517578946B5593F70E27E7661080AB4138C0C9C8656854B9FE989EAB12A8B3A25085C8C2AC9200C185F561E20B336DCA3094E42D827678517DCEA62FCAE524DD3EA9785BEC2E4A550037D3765832F9C491C43BECF490AFE024784F4AA42E941F0252A83DE515C41A14A923EFF4348EBE8242BD2C803AC9CB14A6668799EE112BA06B685A1A85E09454E56AC20A4A2C024A2CA8224F6CB175D5B600CEEE5217F1B602B55DA68A300E0E90ABB0645192EAA8EA1A7B8050F84B3B53D5848160A060B70ABB5DA377838D4F2A53A04A3C34B4654B2B3817D8B558736C8C3D0B55A6A4D7C1EC5495F0B4BB3A1E91522F5C6E6A168AFB5265A63A0600D89958696DC53050A5DF319F80C7FCAEDAAF9F35A8C8EBAE2A95F7A84B4B5E974599BC43B50301D7D95F70BA513867722AEBECCC996554B03167E512CB06A9A2DE21B967157548CA251D922A2A14CF9C4461126745FC93352955EA815CF7717A2045FC1E48A9520FE41686D30329E2F7404A957A2026484E0FA488DF032955EA0164B2BA88DF83026BD5B7F79C1E780C5597EA5884E01967ABC8ED420A1CC0DEC7497AE671045B4BAB6770FED82AF29E15E6B36117E774C9E5CA4631D549C3420C09A55666C2834665463CF17318B64CFE802D17F74B7D674CE0F0438B46F3E667FA3D5A7B88C6C3CFCCE57EAA4E85B2C1DE1083C8057C2E2A9537211DDA75F94368AB8919FAE5A781075E841CA82CF3EC70A91A7C7429BB7F866BF54D304C65E2980C2FBFEC6B920C182153C9E29C80F789CDA1169F2C0DB5FDF8913F5AAA9E059445D7FD4CE3AAA4F3B073FF410A2A3B6AA89A75F45B305AD73A59DBE28BAD21C7C14B107A6259C5AB6A5B54B91E6C7666E70D0F882945C7D76A2096FD1E6E28C513FB24D9A1D5E91B77505435C1EE9AD0DB6A22D94FD9EBB57A94C5E7CE43CD225948B42AA68E3B15428948A6C35CC4C1C68BDFF88B10A8657BFD391DE0397AF676EB944811C920D99A531CE8A72FB201E21A365724A940FA7031B02CD143EB22991D1A53878F6AF322204333FF20181C7C1B5EB56D1659196C0D709993101E2F5DCDE67CF63D64225C7CBCB99FF8E90ECB18EE1453D56C0F59CA1786C36BDDB52F73C0F008819AB65919F421A89A53A50E86CF97C0DCBAB66570FF6480BC1D96AD8F1041145AF10749B94A6443ABBE098823F2F4C880C0151C700AC745448571E0A6CE386214046C98759429276DE3CA903002229DED9234DA98904AD472DF082674575AC2A5F01A3580635FBA293978158B5E58D3896220F60C5353198430FA9180E321723348943675D51BBB5AF4A324642EFD3A915309C40F4154BEA3E5122EE550561F8E7DD9AAE49A5ACE94A8A603D92AF3695591AD1218FDC8D6F110B9B19EF5A9ABDED8951818252173C1D8899C4A20F69FA88FDE13DF7C5696593520D51EE8F9B226BFAD4832D6E7596843E255B76D48E2FB775730A02AAE4822B9AC5168E5EEFA6648721127F68C01AA28E80075D84AFCC1345F5465A8E71F04C3A71DF0AB56966CAC0DEC8B38DBE22142C1B83B2E08D7436CA6BB6EA6F2E259CA81AAB6D73F7D2F5D7DB33F58FEACF2AADABB4EEF6FA854EA24C968458996AC5D137014554703579B66517E20AB9703FD0CBEFD1A4671F2E146B679007CFA5335A74A5D12458D31E046123F127078C2A18D814CD4132619D770AADBE617F89556D59E2E76430A09AF70AAF3072618947840E3204FF3DD99A27069D5B5CD23F443BAAA655DE060E06A02A455973F0C70083CF40721C0A73AEDAF4459E4D4B47930A61E33568DEC2850F400F833CDA9694F53EC6DA07245915FD9F6CC02AA52F3B38B612B4DB1632DB1E7A15369DBC54317E578B730E3602B9B5E8C60BE72B5214B677BA44386F374F3C6AC90D5DB8ECC3E8664191BDCCDCEA02513AD9094DA86F4EE6BC88A325C2DB1B23539CE8EBEFAEE8A00CAD3DE9B44EF9108D9F37B751E68557721E4D8A1175FDD0C5C79EE5BD5A735707E4E48FEB44BDB488C1A3A8F6338ADF92FC8B8751C928ACF28D236023961EB91DC6004EB946051788DDC3561A3DD3B6536D80705007A98DF859C06390785E4EC9AC3D00D39EBA013343981A81286CB59390D1F6F6D9BE5F16317A9FC4524DBA61180A56E06055731248E72523A1E71CCB2DA59224E1D49A4411C202A8821719453B4F1886396E3CD1271EA60270DE200814B0C89A39CB08C471CB38C679688530747691007087B624E9C3A7D9780009C1C5F968CBDBD0C553947158F0C6649AE2CF101B59F7363261912473963138F3866299F2C1187DA9DB9E19E3A46DA30CE5F240BCD61273192758D9D170D8B0563672FB3936147466C3BA97B1C139BDA13A541BD3A12DB38078C8CD87692CB382636255BA571CC4C89AD91A5844B57D34C27C0F081412B108DDEA6F991D8F864FA789403A8127654651F8FF2686FC507FC338D62EF05DD6015629D645F3F1EDDEF70EB0DCA7F9DA32478A9417CC43043E4933E6BA0659DABF0392A739450189555CAE2F235324ABD95977A27711A3C7B7E8A8B7D840F7EC4DFF4B3B7DEE12A179B27B4BA0AEF76E97697E221A3CDD3FAAD490C92EF44D4FFC72306E78F05556D0C01A319E021A0BBF07417AC5715DE97DE3AA136571E08A2C0FD8A42E26E41E612335E8A5EDE2A48B751A808A8205F95FFA57241BA0B1FBC6F888F9B9C866D8A7D3C0FBC97D8DB24058CBA3DFE89D96FB5F9FE9FFF035DFE1BEFDBC30300, '5.0.0.net45')
