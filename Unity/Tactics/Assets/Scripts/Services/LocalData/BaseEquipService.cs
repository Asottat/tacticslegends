using Assets.Scripts.Entities.LocalData;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Services.LocalData
{
    public class BaseEquipService
    {
        private List<BaseEquipModel> _allEquip;

        public BaseEquipService()
        {
            _allEquip = new List<BaseEquipModel>();

            #region Chest (Torso)

            _allEquip.Add(new BaseEquipModel(1,1,2,"0:3:1"));
            _allEquip.Add(new BaseEquipModel(2,1,2,"0:3:2"));
            _allEquip.Add(new BaseEquipModel(3,1,2,"0:3:2","0:4:16"));
            _allEquip.Add(new BaseEquipModel(4,1,1,"0:3:3"));
            _allEquip.Add(new BaseEquipModel(5,1,1,"0:3:3","0:4:20"));
            _allEquip.Add(new BaseEquipModel(6,1,1,"0:3:3","0:4:20","0:5:18"));
            _allEquip.Add(new BaseEquipModel(7,1,1,"0:3:4"));
            _allEquip.Add(new BaseEquipModel(8,1,1,"0:3:4","0:4:15"));
            _allEquip.Add(new BaseEquipModel(9,1,1,"0:3:5"));
            _allEquip.Add(new BaseEquipModel(10,1,1,"0:3:6"));
            _allEquip.Add(new BaseEquipModel(11,1,1,"0:3:6","0:4:20"));
            _allEquip.Add(new BaseEquipModel(12,1,1,"0:3:6","0:4:20","0:5:18"));
            _allEquip.Add(new BaseEquipModel(13,1,1,"0:3:7"));
            _allEquip.Add(new BaseEquipModel(14,1,1,"0:3:7","0:4:20"));
            _allEquip.Add(new BaseEquipModel(15,1,1,"0:3:7","0:4:20","0:5:18"));
            _allEquip.Add(new BaseEquipModel(16,1,1,"0:3:8"));
            _allEquip.Add(new BaseEquipModel(17,1,1,"0:3:9"));
            _allEquip.Add(new BaseEquipModel(18,1,1,"0:3:9","0:4:20"));
            _allEquip.Add(new BaseEquipModel(19,1,1,"0:3:9","0:4:20","0:5:18"));
            _allEquip.Add(new BaseEquipModel(20,1,2,"0:3:10"));
            _allEquip.Add(new BaseEquipModel(21,1,2,"0:3:10","0:4:19"));
            _allEquip.Add(new BaseEquipModel(22,1,2,"0:3:10","0:4:19","0:5:17"));
            _allEquip.Add(new BaseEquipModel(23,1,2,"0:3:11"));
            _allEquip.Add(new BaseEquipModel(24,1,2,"0:3:11","0:4:17"));
            _allEquip.Add(new BaseEquipModel(25,1,2,"0:3:11","0:4:17","0:5:8"));
            _allEquip.Add(new BaseEquipModel(26,1,2,"0:3:12"));
            _allEquip.Add(new BaseEquipModel(27,1,2,"0:3:12","0:4:16"));
            _allEquip.Add(new BaseEquipModel(28,1,2,"0:3:12"));
            _allEquip.Add(new BaseEquipModel(29,1,2,"0:3:12","0:4:16"));
            _allEquip.Add(new BaseEquipModel(30,1,2,"0:3:12","0:4:11"));
            _allEquip.Add(new BaseEquipModel(31,1,2,"0:3:13"));
            _allEquip.Add(new BaseEquipModel(32,1,2,"0:3:13","0:4:11"));
            _allEquip.Add(new BaseEquipModel(33,1,2,"0:3:14"));
            _allEquip.Add(new BaseEquipModel(34,1,2,"0:3:14","0:4:3"));
            _allEquip.Add(new BaseEquipModel(35,1,3,"0:3:15"));
            _allEquip.Add(new BaseEquipModel(36,1,3,"0:3:15","0:4:10"));
            _allEquip.Add(new BaseEquipModel(37,1,3,"0:3:15","0:4:13"));
            _allEquip.Add(new BaseEquipModel(38,1,3,"0:3:15","0:4:13","0:5:12"));
            _allEquip.Add(new BaseEquipModel(39,1,3,"0:3:16"));
            _allEquip.Add(new BaseEquipModel(40,1,3,"0:3:16","0:4:1"));
            _allEquip.Add(new BaseEquipModel(41,1,2,"0:3:17"));
            _allEquip.Add(new BaseEquipModel(42,1,2,"0:3:17","0:4:2"));
            _allEquip.Add(new BaseEquipModel(43,1,2,"0:3:17","0:4:2","0:5:8"));
            _allEquip.Add(new BaseEquipModel(44,1,2,"0:3:17","0:4:18"));
            _allEquip.Add(new BaseEquipModel(45,1,2,"0:3:17","0:4:18","0:5:17"));
            _allEquip.Add(new BaseEquipModel(46,1,3,"0:3:18"));
            _allEquip.Add(new BaseEquipModel(47,1,3,"0:3:18","0:4:8"));
            _allEquip.Add(new BaseEquipModel(48,1,3,"0:3:19"));
            _allEquip.Add(new BaseEquipModel(49,1,3,"0:3:19","0:4:14"));
            _allEquip.Add(new BaseEquipModel(50,1,3,"0:3:19","0:4:14","0:5:7"));
            _allEquip.Add(new BaseEquipModel(51,1,3,"0:3:20"));
            _allEquip.Add(new BaseEquipModel(52,1,3,"0:3:20","0:4:6"));
            _allEquip.Add(new BaseEquipModel(53,1,3,"0:3:20","0:4:6","0:5:6"));
            _allEquip.Add(new BaseEquipModel(54,1,3,"0:3:21"));
            _allEquip.Add(new BaseEquipModel(55,1,3,"0:3:21","0:4:6"));
            _allEquip.Add(new BaseEquipModel(56,1,2,"0:3:22"));
            _allEquip.Add(new BaseEquipModel(57,1,2,"0:3:22","0:4:7"));
            _allEquip.Add(new BaseEquipModel(58,1,2,"0:3:23"));
            _allEquip.Add(new BaseEquipModel(59,1,2,"0:3:23","0:4:11"));
            _allEquip.Add(new BaseEquipModel(60,1,3,"0:3:24"));
            _allEquip.Add(new BaseEquipModel(61,1,3,"0:3:24","0:4:12"));
            _allEquip.Add(new BaseEquipModel(62,1,3,"0:3:24","0:4:12","0:5:4"));
            _allEquip.Add(new BaseEquipModel(63,1,3,"0:3:25"));
            _allEquip.Add(new BaseEquipModel(64,1,3,"0:3:25","0:4:6"));
            _allEquip.Add(new BaseEquipModel(65,1,3,"0:3:25","0:4:6","0:5:6"));
            _allEquip.Add(new BaseEquipModel(66,1,1,"0:3:26"));
            _allEquip.Add(new BaseEquipModel(67,1,1,"0:3:26","0:4:11"));
            _allEquip.Add(new BaseEquipModel(68,1,1,"0:3:27"));
            _allEquip.Add(new BaseEquipModel(69,1,1,"0:3:27","0:4:20"));
            _allEquip.Add(new BaseEquipModel(70,1,1,"0:3:27","0:4:20","0:5:18"));
            _allEquip.Add(new BaseEquipModel(71,1,1,"0:3:28"));
            _allEquip.Add(new BaseEquipModel(72,1,1,"0:3:28","0:4:16"));

            #endregion

            #region Legs (Hips)

            _allEquip.Add(new BaseEquipModel(73,4,2,"0:7:1"));
            _allEquip.Add(new BaseEquipModel(74,4,2,"0:7:2"));
            _allEquip.Add(new BaseEquipModel(75,4,1,"0:7:3"));
            _allEquip.Add(new BaseEquipModel(76,4,1,"0:7:4"));
            _allEquip.Add(new BaseEquipModel(77,4,1,"0:7:5"));
            _allEquip.Add(new BaseEquipModel(78,4,1,"0:7:6"));
            _allEquip.Add(new BaseEquipModel(79,4,1,"0:7:7"));
            _allEquip.Add(new BaseEquipModel(80,4,1,"0:7:8"));
            _allEquip.Add(new BaseEquipModel(81,4,1,"0:7:9"));
            _allEquip.Add(new BaseEquipModel(82,4,2,"0:7:10"));
            _allEquip.Add(new BaseEquipModel(83,4,2,"0:7:11"));
            _allEquip.Add(new BaseEquipModel(84,4,1,"0:7:12"));
            _allEquip.Add(new BaseEquipModel(85,4,2,"0:7:13"));
            _allEquip.Add(new BaseEquipModel(86,4,2,"0:7:14"));
            _allEquip.Add(new BaseEquipModel(87,4,3,"0:7:15"));
            _allEquip.Add(new BaseEquipModel(88,4,3,"0:7:16"));
            _allEquip.Add(new BaseEquipModel(89,4,2,"0:7:17"));
            _allEquip.Add(new BaseEquipModel(90,4,3,"0:7:18"));
            _allEquip.Add(new BaseEquipModel(91,4,3,"0:7:19"));
            _allEquip.Add(new BaseEquipModel(92,4,3,"0:7:20"));
            _allEquip.Add(new BaseEquipModel(93,4,2,"0:7:21"));
            _allEquip.Add(new BaseEquipModel(94,4,3,"0:7:22"));
            _allEquip.Add(new BaseEquipModel(95,4,1,"0:7:23"));
            _allEquip.Add(new BaseEquipModel(96,4,1,"0:7:24"));
            _allEquip.Add(new BaseEquipModel(97,4,3,"0:7:25"));
            _allEquip.Add(new BaseEquipModel(98,4,1,"0:7:26"));
            _allEquip.Add(new BaseEquipModel(99,4,1,"0:7:27"));
            _allEquip.Add(new BaseEquipModel(100,4,1,"0:7:28"));

            #endregion

            #region Hands

            _allEquip.Add(new BaseEquipModel(101,3,1,"0:6:1"));
            _allEquip.Add(new BaseEquipModel(102,3,1,"0:5:13","0:6:1"));
            _allEquip.Add(new BaseEquipModel(103,3,2,"0:6:2"));
            _allEquip.Add(new BaseEquipModel(104,3,2,"0:5:1","0:6:2"));
            _allEquip.Add(new BaseEquipModel(105,3,2,"0:6:3"));
            _allEquip.Add(new BaseEquipModel(106,3,2,"0:5:9","0:6:3"));
            _allEquip.Add(new BaseEquipModel(107,3,2,"0:6:4"));
            _allEquip.Add(new BaseEquipModel(108,3,2,"0:5:3","0:6:4"));
            _allEquip.Add(new BaseEquipModel(109,3,3,"0:6:5"));
            _allEquip.Add(new BaseEquipModel(110,3,3,"0:5:4","0:6:5"));
            _allEquip.Add(new BaseEquipModel(111,3,3,"0:6:6"));
            _allEquip.Add(new BaseEquipModel(112,3,1,"0:6:7"));
            _allEquip.Add(new BaseEquipModel(113,3,2,"0:5:8","0:6:7"));
            _allEquip.Add(new BaseEquipModel(114,3,3,"0:5:12","0:6:7"));
            _allEquip.Add(new BaseEquipModel(115,3,1,"0:5:13","0:6:7"));
            _allEquip.Add(new BaseEquipModel(116,3,1,"0:6:8"));
            _allEquip.Add(new BaseEquipModel(117,3,1,"0:5:14","0:6:8"));
            _allEquip.Add(new BaseEquipModel(118,3,2,"0:6:9"));
            _allEquip.Add(new BaseEquipModel(119,3,2,"0:5:2","0:6:9"));
            _allEquip.Add(new BaseEquipModel(120,3,2,"0:5:10","0:6:9"));
            _allEquip.Add(new BaseEquipModel(121,3,1,"0:6:10"));
            _allEquip.Add(new BaseEquipModel(122,3,1,"0:5:13","0:6:10"));
            _allEquip.Add(new BaseEquipModel(123,3,3,"0:6:11"));
            _allEquip.Add(new BaseEquipModel(124,3,3,"0:5:5","0:6:11"));
            _allEquip.Add(new BaseEquipModel(125,3,3,"0:6:12"));
            _allEquip.Add(new BaseEquipModel(126,3,3,"0:5:7","0:6:12"));
            _allEquip.Add(new BaseEquipModel(127,3,3,"0:5:11","0:6:12"));
            _allEquip.Add(new BaseEquipModel(128,3,2,"0:6:13"));
            _allEquip.Add(new BaseEquipModel(129,3,2,"0:6:14"));
            _allEquip.Add(new BaseEquipModel(130,3,2,"0:5:10","0:6:14"));
            _allEquip.Add(new BaseEquipModel(131,3,2,"0:5:15","0:6:14"));
            _allEquip.Add(new BaseEquipModel(132,3,1,"0:6:15"));
            _allEquip.Add(new BaseEquipModel(133,3,2,"0:5:1","0:6:16"));
            _allEquip.Add(new BaseEquipModel(134,3,2,"0:6:17"));
            _allEquip.Add(new BaseEquipModel(135,3,2,"0:5:9","0:6:17"));


            #endregion

            #region Wrists

            _allEquip.Add(new BaseEquipModel(136,2,2,"0:5:1"));
            _allEquip.Add(new BaseEquipModel(137,2,2,"0:5:2"));
            _allEquip.Add(new BaseEquipModel(138,2,2,"0:5:3"));
            _allEquip.Add(new BaseEquipModel(139,2,3,"0:5:4"));
            _allEquip.Add(new BaseEquipModel(140,2,3,"0:5:5"));
            _allEquip.Add(new BaseEquipModel(141,2,3,"0:5:6"));
            _allEquip.Add(new BaseEquipModel(142,2,3,"0:5:7"));
            _allEquip.Add(new BaseEquipModel(143,2,2,"0:5:8"));
            _allEquip.Add(new BaseEquipModel(144,2,2,"0:5:9"));
            _allEquip.Add(new BaseEquipModel(145,2,2,"0:5:10"));
            _allEquip.Add(new BaseEquipModel(146,2,3,"0:5:11"));
            _allEquip.Add(new BaseEquipModel(147,2,3,"0:5:12"));
            _allEquip.Add(new BaseEquipModel(148,2,1,"0:5:13"));
            _allEquip.Add(new BaseEquipModel(149,2,1,"0:5:14"));
            _allEquip.Add(new BaseEquipModel(150,2,2,"0:5:15"));
            _allEquip.Add(new BaseEquipModel(151,2,3,"0:5:16"));
            _allEquip.Add(new BaseEquipModel(152,2,3,"0:5:17"));
            _allEquip.Add(new BaseEquipModel(153,2,1,"0:5:18"));

            #endregion

            #region Feet

            _allEquip.Add(new BaseEquipModel(154,5,2,"0:8:1"));
            _allEquip.Add(new BaseEquipModel(155,5,3,"0:8:2"));
            _allEquip.Add(new BaseEquipModel(156,5,3,"0:8:2","2:8:7"));
            _allEquip.Add(new BaseEquipModel(157,5,1,"0:8:3"));
            _allEquip.Add(new BaseEquipModel(158,5,1,"0:8:4"));
            _allEquip.Add(new BaseEquipModel(159,5,2,"0:8:5"));
            _allEquip.Add(new BaseEquipModel(160,5,3,"0:8:6"));
            _allEquip.Add(new BaseEquipModel(161,5,2,"0:8:7"));
            _allEquip.Add(new BaseEquipModel(162,5,2,"0:8:7","2:8:5"));
            _allEquip.Add(new BaseEquipModel(163,5,3,"0:8:8"));
            _allEquip.Add(new BaseEquipModel(164,5,2,"0:8:9"));
            _allEquip.Add(new BaseEquipModel(165,5,2,"0:8:10"));
            _allEquip.Add(new BaseEquipModel(166,5,1,"0:8:11"));
            _allEquip.Add(new BaseEquipModel(167,5,3,"0:8:12"));
            _allEquip.Add(new BaseEquipModel(168,5,3,"0:8:13"));
            _allEquip.Add(new BaseEquipModel(169,5,3,"0:8:14"));
            _allEquip.Add(new BaseEquipModel(170,5,3,"0:8:14","2:8:1"));
            _allEquip.Add(new BaseEquipModel(171,5,3,"0:8:14","2:8:2"));
            _allEquip.Add(new BaseEquipModel(172,5,3,"0:8:14","2:8:6"));
            _allEquip.Add(new BaseEquipModel(173,5,3,"0:8:14","2:8:7"));
            _allEquip.Add(new BaseEquipModel(174,5,3,"0:8:14","2:8:8"));
            _allEquip.Add(new BaseEquipModel(175,5,3,"0:8:14","2:8:9"));
            _allEquip.Add(new BaseEquipModel(176,5,3,"0:8:15"));
            _allEquip.Add(new BaseEquipModel(177,5,3,"0:8:15","2:8:1"));
            _allEquip.Add(new BaseEquipModel(178,5,3,"0:8:16"));
            _allEquip.Add(new BaseEquipModel(179,5,3,"0:8:16","2:8:1"));
            _allEquip.Add(new BaseEquipModel(180,5,3,"0:8:16","2:8:2"));
            _allEquip.Add(new BaseEquipModel(181,5,3,"0:8:16","2:8:4"));
            _allEquip.Add(new BaseEquipModel(182,5,1,"0:8:17"));
            _allEquip.Add(new BaseEquipModel(183,5,2,"0:8:18"));
            _allEquip.Add(new BaseEquipModel(184,5,1,"0:8:19"));

            #endregion

            #region Head

            _allEquip.Add(new BaseEquipModel(185,0,2,"0:0:1:1"));
            _allEquip.Add(new BaseEquipModel(186,0,3,"0:0:1:2"));
            _allEquip.Add(new BaseEquipModel(187,0,3,"0:0:1:2","2:2:1"));
            _allEquip.Add(new BaseEquipModel(188,0,3,"0:0:1:2","2:2:2"));
            _allEquip.Add(new BaseEquipModel(189,0,3,"0:0:1:2","2:2:3"));
            _allEquip.Add(new BaseEquipModel(190,0,3,"0:0:1:3","2:2:5"));
            _allEquip.Add(new BaseEquipModel(191,0,3,"0:0:1:4"));
            _allEquip.Add(new BaseEquipModel(192,0,3,"0:0:1:4","2:2:1"));
            _allEquip.Add(new BaseEquipModel(193,0,3,"0:0:1:4","2:2:2"));
            _allEquip.Add(new BaseEquipModel(194,0,3,"0:0:1:4","2:2:6"));
            _allEquip.Add(new BaseEquipModel(195,0,3,"0:0:1:5"));
            _allEquip.Add(new BaseEquipModel(196,0,3,"0:0:1:5","2:2:1"));
            _allEquip.Add(new BaseEquipModel(197,0,3,"0:0:1:5","2:2:2"));
            _allEquip.Add(new BaseEquipModel(198,0,3,"0:0:1:5","2:2:6"));
            _allEquip.Add(new BaseEquipModel(199,0,3,"0:0:1:6","2:2:1"));
            _allEquip.Add(new BaseEquipModel(200,0,3,"0:0:1:6","2:2:2"));
            _allEquip.Add(new BaseEquipModel(201,0,3,"0:0:1:6","2:2:4"));
            _allEquip.Add(new BaseEquipModel(202,0,3,"0:0:1:6","2:2:6"));
            _allEquip.Add(new BaseEquipModel(203,0,3,"0:0:1:6","2:2:7"));
            _allEquip.Add(new BaseEquipModel(204,0,3,"0:0:1:7"));
            _allEquip.Add(new BaseEquipModel(205,0,3,"0:0:1:8"));
            _allEquip.Add(new BaseEquipModel(206,0,3,"0:0:1:8","2:2:3"));
            _allEquip.Add(new BaseEquipModel(207,0,3,"0:0:1:9"));
            _allEquip.Add(new BaseEquipModel(208,0,3,"0:0:1:9","2:2:1"));
            _allEquip.Add(new BaseEquipModel(209,0,3,"0:0:1:9","2:2:2"));
            _allEquip.Add(new BaseEquipModel(210,0,3,"0:0:1:9","2:2:4"));
            _allEquip.Add(new BaseEquipModel(211,0,3,"0:0:1:9","2:2:5"));
            _allEquip.Add(new BaseEquipModel(212,0,3,"0:0:1:10"));
            _allEquip.Add(new BaseEquipModel(213,0,3,"0:0:1:10","2:2:1"));
            _allEquip.Add(new BaseEquipModel(214,0,3,"0:0:1:10","2:2:2"));
            _allEquip.Add(new BaseEquipModel(215,0,3,"0:0:1:10","2:2:5"));
            _allEquip.Add(new BaseEquipModel(216,0,3,"0:0:1:11"));
            _allEquip.Add(new BaseEquipModel(217,0,3,"0:0:1:11","2:2:11"));
            _allEquip.Add(new BaseEquipModel(218,0,3,"0:0:1:12"));
            _allEquip.Add(new BaseEquipModel(219,0,3,"0:0:1:12","2:2:1"));
            _allEquip.Add(new BaseEquipModel(220,0,3,"0:0:1:12","2:2:2"));
            _allEquip.Add(new BaseEquipModel(221,0,3,"0:0:1:13"));
            _allEquip.Add(new BaseEquipModel(222,0,3,"0:0:1:13","2:2:4"));
            _allEquip.Add(new BaseEquipModel(223,0,3,"0:0:1:13","2:2:5"));
            _allEquip.Add(new BaseEquipModel(224,0,3,"0:0:1:13","2:2:6"));
            _allEquip.Add(new BaseEquipModel(225,0,3,"0:0:1:13","2:2:10"));
            _allEquip.Add(new BaseEquipModel(226,0,3,"0:0:1:13","2:2:12"));
            _allEquip.Add(new BaseEquipModel(227,0,3,"0:0:1:13","2:2:13"));
            _allEquip.Add(new BaseEquipModel(228,0,1,"2:0:2:1"));
            _allEquip.Add(new BaseEquipModel(229,0,3,"2:0:2:2"));
            _allEquip.Add(new BaseEquipModel(230,0,2,"2:0:2:3"));
            _allEquip.Add(new BaseEquipModel(231,0,3,"2:0:2:4"));
            _allEquip.Add(new BaseEquipModel(232,0,1,"2:0:2:5"));
            _allEquip.Add(new BaseEquipModel(233,0,1,"2:0:2:6"));
            _allEquip.Add(new BaseEquipModel(234,0,2,"2:0:2:7"));
            _allEquip.Add(new BaseEquipModel(235,0,2,"2:0:2:8"));
            _allEquip.Add(new BaseEquipModel(236,0,2,"2:0:2:9"));
            _allEquip.Add(new BaseEquipModel(237,0,2,"2:0:2:10"));
            _allEquip.Add(new BaseEquipModel(238,0,1,"2:0:2:11"));
            _allEquip.Add(new BaseEquipModel(239,0,3,"2:0:2:12"));
            _allEquip.Add(new BaseEquipModel(240,0,2,"2:0:2:13"));
            _allEquip.Add(new BaseEquipModel(241,0,1,"2:0:0:1"));
            _allEquip.Add(new BaseEquipModel(242,0,1,"2:0:0:2"));
            _allEquip.Add(new BaseEquipModel(243,0,1,"2:0:0:3"));
            _allEquip.Add(new BaseEquipModel(244,0,1,"2:0:0:4"));
            _allEquip.Add(new BaseEquipModel(245,0,1,"2:0:0:5"));
            _allEquip.Add(new BaseEquipModel(246,0,1,"2:0:0:6"));
            _allEquip.Add(new BaseEquipModel(247,0,1,"2:0:0:7"));
            _allEquip.Add(new BaseEquipModel(248,0,1,"2:0:0:8"));
            _allEquip.Add(new BaseEquipModel(249,0,1,"2:0:0:9"));
            _allEquip.Add(new BaseEquipModel(250,0,1,"2:0:0:10"));
            _allEquip.Add(new BaseEquipModel(251,0,1,"2:0:0:11"));
            _allEquip.Add(new BaseEquipModel(252,0,2,"2:0:1:1"));
            _allEquip.Add(new BaseEquipModel(253,0,2,"2:0:1:2"));
            _allEquip.Add(new BaseEquipModel(254,0,1,"2:0:1:3"));
            _allEquip.Add(new BaseEquipModel(255,0,1,"2:0:1:4"));

            #endregion

            #region Back

            _allEquip.Add(new BaseEquipModel(256,7,"2:4:1"));
            _allEquip.Add(new BaseEquipModel(257,7,"2:4:2"));
            _allEquip.Add(new BaseEquipModel(258,7,"2:4:3"));
            _allEquip.Add(new BaseEquipModel(259,7,"2:4:4"));
            _allEquip.Add(new BaseEquipModel(260,7,"2:4:5"));
            _allEquip.Add(new BaseEquipModel(261,7,"2:4:6"));
            _allEquip.Add(new BaseEquipModel(262,7,"2:4:7"));
            _allEquip.Add(new BaseEquipModel(263,7,"2:4:8"));
            _allEquip.Add(new BaseEquipModel(264,7,"2:4:9"));
            _allEquip.Add(new BaseEquipModel(265,7,"2:4:10"));
            _allEquip.Add(new BaseEquipModel(266,7,"2:4:11"));
            _allEquip.Add(new BaseEquipModel(267,7,"2:4:12"));
            _allEquip.Add(new BaseEquipModel(268,7,"2:4:13"));
            _allEquip.Add(new BaseEquipModel(269,7,"2:4:14"));
            _allEquip.Add(new BaseEquipModel(270,7,"2:4:15"));

            #endregion

            #region Shoulder

            _allEquip.Add(new BaseEquipModel(271,6,3,"2:5:1"));
            _allEquip.Add(new BaseEquipModel(272,6,3,"2:5:2"));
            _allEquip.Add(new BaseEquipModel(273,6,3,"2:5:3"));
            _allEquip.Add(new BaseEquipModel(274,6,3,"2:5:4"));
            _allEquip.Add(new BaseEquipModel(275,6,3,"2:5:5"));
            _allEquip.Add(new BaseEquipModel(276,6,2,"2:5:6"));
            _allEquip.Add(new BaseEquipModel(277,6,3,"2:5:7"));
            _allEquip.Add(new BaseEquipModel(278,6,3,"2:5:8"));
            _allEquip.Add(new BaseEquipModel(279,6,2,"2:5:9"));
            _allEquip.Add(new BaseEquipModel(280,6,3,"2:5:10"));
            _allEquip.Add(new BaseEquipModel(281,6,3,"2:5:11"));
            _allEquip.Add(new BaseEquipModel(282,6,3,"2:5:12"));
            _allEquip.Add(new BaseEquipModel(283,6,3,"2:5:13"));
            _allEquip.Add(new BaseEquipModel(284,6,3,"2:5:14"));
            _allEquip.Add(new BaseEquipModel(285,6,2,"2:5:15"));
            _allEquip.Add(new BaseEquipModel(286,6,2,"2:5:16"));
            _allEquip.Add(new BaseEquipModel(287,6,3,"2:5:17"));
            _allEquip.Add(new BaseEquipModel(288,6,3,"2:5:18"));
            _allEquip.Add(new BaseEquipModel(289,6,3,"2:5:19"));
            _allEquip.Add(new BaseEquipModel(290,6,3,"2:5:20"));
            _allEquip.Add(new BaseEquipModel(291,6,3,"2:5:21"));

            #endregion
        }

        public BaseEquipModel GetById(int id)
        {
            return _allEquip.FirstOrDefault(f => f.Id == id);
        }

        public List<BaseEquipModel> GetBaseEquipBySlot(ItemSlotType slotType)
        {
            return _allEquip.Where(w => w.Slot == (int)slotType).ToList();
        }

        public List<BaseEquipModel> GetBaseEquipBySlotClass(ItemSlotType slotType, ItemClassType classType)
        {
            int st = (int)slotType;
            int ct = (int)classType;
            return _allEquip.Where(w => w.Slot == st && w.ClassType.HasValue && w.ClassType == ct).ToList();
        }
    }
}
