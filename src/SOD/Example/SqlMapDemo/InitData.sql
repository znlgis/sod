/** 
    执行此脚本之前，请先看看项目根目录下有没有数据库文件 TestDB.mdf，没有用VS创建一个，然后在VS的解决方案文件夹双击此数据库文件 ，再执行本文件的脚本 
	注意：项目中原先自带的 TestDB.mdf采用VS2013创建，如果你使用更高版本的VS，可能无法访问此文件，请删除该数据库文件再用你的VS重新创建它，
	设置文件的属性为复制到输出目录，然后修改连接字符串，连接字符串可以通过双击数据库文件在服务器资源管理其--数据连接--当前数据库名称--右键属性里面看到。

    本文件内容来自博客 http://www.cnblogs.com/tianxue/p/4493260.html
	相关博客请参考    https://www.cnblogs.com/bluedoctor/archive/2011/05/06/2038727.html
	整理时间：2018.12.7
**/
/****** Object:  Table [dbo].[Score]    Script Date: 05/11/2015 23:16:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Score](
    [stuID] [int] NOT NULL,
    [category] [nvarchar](50) NOT NULL,
    [score] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (1, N'英语', 80)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (2, N'数学', 80)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (1, N'数学', 70)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (2, N'英语', 89)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (3, N'英语', 81)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (3, N'数学', 71)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (4, N'数学', 91)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (4, N'英语', 61)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (5, N'英语', 91)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (6, N'英语', 89)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (7, N'英语', 77)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (8, N'英语', 97)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (9, N'英语', 57)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (5, N'数学', 87)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (6, N'数学', 89)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (7, N'数学', 80)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (8, N'数学', 81)
INSERT [dbo].[Score] ([stuID], [category], [score]) VALUES (9, N'数学', 84)
/****** Object:  Table [dbo].[Department]    Script Date: 05/11/2015 23:16:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department](
    [depID] [int] IDENTITY(1,1) NOT NULL,
    [depName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [depID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Department] ON
INSERT [dbo].[Department] ([depID], [depName]) VALUES (1, N'计算机')
INSERT [dbo].[Department] ([depID], [depName]) VALUES (2, N'生物')
INSERT [dbo].[Department] ([depID], [depName]) VALUES (3, N'数学')
SET IDENTITY_INSERT [dbo].[Department] OFF
/****** Object:  Table [dbo].[Student]    Script Date: 05/11/2015 23:16:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student](
    [stuID] [int] IDENTITY(1,1) NOT NULL,
    [stuName] [nvarchar](50) NOT NULL,
    [deptID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
    [stuID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Student] ON
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (1, N'计算机张三', 1)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (2, N'计算机李四', 1)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (3, N'计算机王五', 1)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (4, N'生物amy', 2)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (5, N'生物kity', 2)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (6, N'生物lucky', 2)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (7, N'数学_yiming', 3)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (8, N'数学_haoxue', 3)
INSERT [dbo].[Student] ([stuID], [stuName], [deptID]) VALUES (9, N'数学_wuyong', 3)
SET IDENTITY_INSERT [dbo].[Student] OFF
/****** Object:  Default [DF__Departmen__depNa__5441852A]    Script Date: 05/11/2015 23:16:23 ******/
ALTER TABLE [dbo].[Department] ADD  DEFAULT ('') FOR [depName]
GO
/****** Object:  Default [DF__Score__category__5EBF139D]    Script Date: 05/11/2015 23:16:23 ******/
ALTER TABLE [dbo].[Score] ADD  DEFAULT ('') FOR [category]
GO
/****** Object:  Default [DF__Score__score__5FB337D6]    Script Date: 05/11/2015 23:16:23 ******/
ALTER TABLE [dbo].[Score] ADD  DEFAULT ((0)) FOR [score]
GO
/****** Object:  Default [DF__Student__stuName__59063A47]    Script Date: 05/11/2015 23:16:23 ******/
ALTER TABLE [dbo].[Student] ADD  DEFAULT ('') FOR [stuName]
GO
/****** Object:  ForeignKey [FK__Student__deptID__59FA5E80]    Script Date: 05/11/2015 23:16:23 ******/
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([deptID])
REFERENCES [dbo].[Department] ([depID])
GO

