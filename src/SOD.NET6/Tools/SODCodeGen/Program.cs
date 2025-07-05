/* SODCodeGen 多层代码生成器
     * 生成WebAPI、Service、Repository 各层使用SOD框架的基础代码，需要结合EntityCreateTool使用，先生成实体类
     * 
     * 命令行参数:
     * -Namespace [your.namespace] [默认值: sod.pwmis.com]
     * -BaseName <base classname>  
     * -PInterface [Interface path] [默认值: /Interfaces ]
     * -PWeb [WebAPI path]         [默认值: /Web/Controllers ]
     * -PService [Service path]    [默认值: /Services/Implements ]
     * -PRepo [Repository path]    [默认值: /Repositories/Implements ]
     */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SODCodeGen
{
    

    internal class Program
    {
        static void Main(string[] args)
        {
            string arg_namespace = "demo.pwmis.com";
            string arg_basename = "MyDemo";
            string arg_pinterface = "/Interfaces";
            string arg_pweb = "/Web/Controllers";
            string arg_pservice = "/Services/Implements";
            string arg_prepo = "/Repositories/Implements";
            if(args.Length > 0 )
            {
                for( int i = 0; i < args.Length-1; i++ )
                {
                    if (args[i] == "-BaseName")
                    {
                        arg_basename = args[i + 1];
                    }
                    else if (args[i] == "-Namespace")
                    {
                        arg_namespace = args[i + 1];
                    }
                    else if (args[i] == "-PInterface")
                    {
                        arg_pinterface = args[i + 1];
                    }
                    else if (args[i] == "-PWeb")
                    {
                        arg_pweb = args[i + 1];
                    }
                    else if (args[i] == "-PService")
                    {
                        arg_pservice = args[i + 1];
                    }
                    else if (args[i] == "-PRepo")
                    {
                        arg_prepo = args[i + 1];
                    }
                    else
                    {
                        Console.WriteLine("参数不正确");
                        return;
                    }
                    i++;
                }
            }
            //
            Console.WriteLine("*-------------------------------------------------*");
            Console.WriteLine("|      SOD WebAPI 多层项目代码生成器 Ver 1.0      |");
            Console.WriteLine("|          Power by bluedoctor,2025-1-1           |");
            Console.WriteLine("|             http://www.pwmis.com/sod            |");
            Console.WriteLine("|             [开源协议 LGPL 2.0]                 |");
            Console.WriteLine("*-------------------------------------------------* ");
            Console.WriteLine("基础类名称：{0}",arg_basename);
            Console.WriteLine("名字空间：{0}",arg_namespace);
            Console.WriteLine("输出文件：接口 目录：{0}",arg_pinterface);
            Console.WriteLine("输出文件：WebAPI 目录：{0}",arg_pweb);
            Console.WriteLine("输出文件：服务 目录：{0}",arg_pservice);
            Console.WriteLine("输出文件：仓储 目录：{0}",arg_prepo);
            Console.WriteLine("开始生成代码...");

            string rootPath =System.IO.Path.GetDirectoryName( Environment.ProcessPath);
            string[] templateFiels = new string[10];
            templateFiels[0] = @"Common\Dtos\SodDemo.cs";
            templateFiels[1] = @"Common\Interface\ISodDemo.cs";
            templateFiels[2] = @"Interfaces\IRepositories\ISodDemoRep.cs";
            templateFiels[3] = @"Interfaces\IServices\ISodDemoService.cs";
            templateFiels[4] = @"Repositories\Entities\SodDemo.cs";
            templateFiels[5] = @"Repositories\Implements\SodDemoRep.cs";
            templateFiels[6] = @"Repositories\Implements\SodDemoRep.designer.cs";
            templateFiels[7] = @"Services\Implements\SodDemoService.cs";
            templateFiels[8] = @"Services\Implements\SodDemoService.designer.cs";
            templateFiels[9] = @"Web\Controllers\SodDemoController.cs";

            foreach(string fiel in templateFiels)
            {
                string filePath= System.IO.Path.Combine(rootPath, fiel);
                if (System.IO.File.Exists(filePath))
                {
                    //Console.WriteLine($"{fiel}");
                    string targetFileName = fiel.Replace("SodDemo", arg_basename);
                    string targetFilePath= System.IO.Path.Combine(rootPath, targetFileName);
                    string fileContent= System.IO.File.ReadAllText(filePath);
                    string targetFileContent= fileContent.Replace("SodDemo", arg_basename).Replace("Sod.Pwmis.com", arg_namespace);
                    System.IO.File.WriteAllText(targetFilePath, targetFileContent);
                    Console.WriteLine(targetFilePath);
                }
            }
                
            Console.WriteLine("完成！");
            Console.ReadLine();

        }
    }
}
