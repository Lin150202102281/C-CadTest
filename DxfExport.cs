using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Linq;
using WW.Cad.Base;
using WW.Cad.Drawing;
using WW.Cad.Drawing.GDI;
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;

namespace Cadl

{
    public class CADEntity
    {
        
        public string type;
        public int color;
        public string transform;
        public ulong calCount;
    }
    public class CADLine : CADEntity
    {
        public string startPoint;
        public string endPoint;
        public int lineWeight;

    }

    public class CADLwPolyLine : CADEntity
    {
        public string[] vertices;
        public string closed;
    }

    public class CADSpline : CADEntity
    {
        public string[] fitPoints;
    }

    public class CADCircle : CADEntity
    {
        public string center;
        public double radius;
    }

    public class CADArc : CADEntity
    {
        public string center;
        public double radius;
        public double startAngle;
        public double endAngle;
    }

    public class CADEllipse : CADEntity
    {
        public string center;
        public string majorAxisEndPoint;
        public string minorAxisEndPoint;
        public double startParameter;
        public double endParameter;
    }

    public class CADMText : CADEntity
    {
        public string simplifiedText;
        public string fontStyle;
        public double size;

        public string attachmentPoint;
        public double boxHeight;
        public double boxWidth;
    }

    public class CADText : CADEntity
    {
        public string simplifiedText;
        public string fontStyle;
        public double size;

        public string alignMentPoint1;
        public double rotationAngle; 
    }

    public class CADInsert : CADEntity
    {
        public string insertPoint;
        public double rotationAngle;
    }


    public class DxfExport
    {
        static List<CADEntity> cadEntities = new List<CADEntity>();

        static FileStream fs = new FileStream("D:\\C项目\\CadLCmd\\CadLCmd\\txt\\test3.txt", FileMode.Create);

        static StreamWriter sw = new StreamWriter(fs, Encoding.Default);

        static DxfLine dxfLine = null;

        static DxfLwPolyline dxfLwPolyline = null;

        static DxfXLine dxfXLine = null;

        static DxfSpline dxfSpline = null;

        static DxfCircle dxfCircle = null;

        static DxfArc dxfArc = null;

        static DxfEllipse dxfEllipse = null;

        static DxfMText dxfMText = null;

        static DxfText dxfText = null;

        static DxfInsert dxfInsert = null;

        static DxfAttributeDefinition dxfAttributeDefinition = null;

        static DxfEntityCollection dxfEntityCollection = null;

        static DxfBlockBegin dxfBlockBegin = null;

        static DxfBlock dxfBlock = null;

        static DxfMText[] KTCodes = new DxfMText[0]; //机型逗号拆分
        static List<DxfMText> ktls = KTCodes.ToList();

        static DxfText[] KTest = new DxfText[0]; //机型逗号拆分
        static List<DxfText> ktest = KTest.ToList();

        static String[] DxfType = new String[0];
        static List<String> dxfType = DxfType.ToList();

        static DxfInsert[] KInsert = new DxfInsert[0]; //机型逗号拆分
        static List<DxfInsert> kins = KInsert.ToList();

        static DxfEntityCollection[] KEC = new DxfEntityCollection[0]; //机型逗号拆分
        static List<DxfEntityCollection> kco = KEC.ToList();

        


        static void FindEntities(DxfEntityCollection Entities)
        {
            
            foreach (var entityGroups in Entities)
            {

                //Console.WriteLine(entityGroups.GetType());
                dxfType.Add(entityGroups.GetType().Name);
                if (typeof(DxfLine) == entityGroups.GetType())
                {
                    dxfLine = entityGroups as DxfLine;
                    //开始写入
                    sw.Write(dxfLine.GetType());
                    sw.WriteLine();
                    sw.Write("开始点:");
                    sw.Write(dxfLine.Start);
                    sw.WriteLine();
                    sw.Write("结束点:");
                    sw.Write(dxfLine.End);
                    sw.WriteLine();
                    sw.Write("线粗:");
                    sw.Write(dxfLine.LineWeight);
                    sw.WriteLine();
                    sw.Write("颜色:");
                    sw.Write(dxfLine.Color.Rgb);
                    sw.WriteLine();
                    sw.Write("变换矩阵:");
                    sw.Write(dxfLine.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();


                    CADLine cADLine = new CADLine
                    {
                        calCount = dxfLine.OwnerObjectSoftReference.Handle,
                        type = dxfLine.GetType().Name,
                        color = dxfLine.Color.Rgb,
                        transform = dxfLine.Transform.DebugString,
                        startPoint = dxfLine.Start.ToString(),
                        endPoint = dxfLine.End.ToString(),
                        lineWeight = dxfLine.LineWeight
                    };

                    cadEntities.Add(cADLine);
                }


                if (typeof(DxfLwPolyline) == entityGroups.GetType())
                {
                    //Console.WriteLine(dxfLwPolyline);

                    dxfLwPolyline = entityGroups as DxfLwPolyline;
                    //开始写入
                    sw.Write(dxfLwPolyline.GetType());
                    sw.WriteLine();

                    string[] arrVertices = new string[dxfLwPolyline.Vertices.Count];
                    for (int i = 0; i < dxfLwPolyline.Vertices.Count; i++)
                    {
                        arrVertices[i] = dxfLwPolyline.Vertices[i].ToString();
                        sw.Write("经过点:");
                        sw.Write(dxfLwPolyline.Vertices[i]);
                        sw.WriteLine();

                    }
                    sw.Write("颜色:");
                    sw.Write(dxfLwPolyline.Color.Rgb);
                    sw.WriteLine();
                    sw.Write("是否关闭:");
                    sw.Write(dxfLwPolyline.Closed);
                    sw.WriteLine();
                    sw.Write("变换矩阵:");
                    sw.Write(dxfLwPolyline.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();


                    CADLwPolyLine cADLwPolyLine = new CADLwPolyLine
                    {
                        calCount = dxfLwPolyline.OwnerObjectSoftReference.Handle,
                        type = dxfLwPolyline.GetType().Name,
                        color = dxfLwPolyline.Color.Rgb,
                        transform = dxfLwPolyline.Transform.DebugString,

                        vertices = arrVertices,
                        closed = dxfLwPolyline.Closed.ToString(),
                    };

                    cadEntities.Add(cADLwPolyLine);



                }


                if (typeof(DxfXLine) == entityGroups.GetType())
                {

                    dxfXLine = entityGroups as DxfXLine;
                    sw.Write(dxfXLine.GetType());

                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                }

                if (typeof(DxfSpline) == entityGroups.GetType())
                {

                    dxfSpline = entityGroups as DxfSpline;
                    sw.Write(dxfSpline.GetType());
                    sw.WriteLine();
                    string[] arrFitPoints = new string[dxfSpline.FitPoints.Count];
                    for (int i = 0; i < dxfSpline.FitPoints.Count; i++)
                    {
                        arrFitPoints[i] = dxfSpline.FitPoints[i].ToString();
                        sw.Write("经过点:");
                        sw.Write(dxfSpline.FitPoints[i]);
                        sw.WriteLine();

                    }
                    sw.Write(dxfSpline.Color.Rgb);
                    sw.WriteLine();
                    sw.Write(dxfSpline.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();


                    CADSpline cADSpline = new CADSpline
                    {
                        calCount = dxfSpline.OwnerObjectSoftReference.Handle,
                        type = dxfSpline.GetType().Name,
                        color = dxfSpline.Color.Rgb,
                        transform = dxfSpline.Transform.DebugString,

                        fitPoints = arrFitPoints,
                    };

                    cadEntities.Add(cADSpline);

                }


                if (typeof(DxfCircle) == entityGroups.GetType())
                {

                    dxfCircle = entityGroups as DxfCircle;
                    sw.Write(dxfCircle.GetType());
                    sw.WriteLine();
                    sw.Write("圆心:");
                    sw.Write(dxfCircle.Center);
                    sw.WriteLine();
                    sw.Write("半径:");
                    sw.Write(dxfCircle.Radius);
                    sw.WriteLine();
                    sw.Write("颜色:");
                    sw.Write(dxfCircle.Color.Rgb);
                    sw.WriteLine();
                    sw.Write("变换矩阵:");
                    sw.Write(dxfCircle.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();



                    CADCircle cADCircle = new CADCircle
                    {
                        calCount = dxfCircle.OwnerObjectSoftReference.Handle,
                        type = dxfCircle.GetType().Name,
                        color = dxfCircle.Color.Rgb,
                        transform = dxfCircle.Transform.DebugString,

                        center = dxfCircle.Center.ToString(),
                        radius = dxfCircle.Radius
                    };

                    cadEntities.Add(cADCircle);



                }

                if (typeof(DxfArc) == entityGroups.GetType())
                {
                    dxfArc = entityGroups as DxfArc;
                    sw.Write(dxfArc.GetType());
                    sw.WriteLine();
                    sw.Write("圆心:");
                    sw.Write(dxfArc.Center);
                    sw.WriteLine();
                    sw.Write("半径:");
                    sw.Write(dxfArc.Radius);
                    sw.WriteLine();
                    sw.Write("开始角度:");
                    sw.Write(dxfArc.StartAngle);
                    sw.WriteLine();
                    sw.Write("结束角度:");
                    sw.Write(dxfArc.EndAngle);
                    sw.WriteLine();
                    sw.Write("颜色:");
                    sw.Write(dxfArc.Color.Rgb);
                    sw.WriteLine();
                    sw.Write("变换矩阵:");
                    sw.Write(dxfArc.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();


                    CADArc cADArc = new CADArc
                    {
                        calCount = dxfArc.OwnerObjectSoftReference.Handle,
                        type = dxfArc.GetType().Name,
                        color = dxfArc.Color.Rgb,
                        transform = dxfArc.Transform.DebugString,

                        center = dxfArc.Center.ToString(),
                        startAngle = dxfArc.StartAngle,
                        endAngle = dxfArc.EndAngle,
                        radius = dxfArc.Radius

                    };

                    cadEntities.Add(cADArc);



                }

                if (typeof(DxfEllipse) == entityGroups.GetType())
                {
                    dxfEllipse = entityGroups as DxfEllipse;

                    sw.Write(dxfEllipse.GetType());
                    sw.WriteLine();
                    sw.Write("椭圆心:");
                    sw.Write(dxfEllipse.Center);
                    sw.WriteLine();
                    sw.Write("椭圆X轴半径:");
                    sw.Write(dxfEllipse.MajorAxisEndPoint);
                    sw.WriteLine();
                    sw.Write("椭圆Y轴半径:");
                    sw.Write(dxfEllipse.MinorAxisEndPoint);
                    sw.WriteLine();
                    sw.Write("开始角:");
                    sw.Write(dxfEllipse.StartParameter);
                    sw.WriteLine();
                    sw.Write("结束角:");
                    sw.Write(dxfEllipse.EndParameter);
                    sw.WriteLine();
                    sw.Write("颜色:");
                    sw.Write(dxfEllipse.Color.Rgb);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();


                    CADEllipse cADEllipse = new CADEllipse
                    {
                        calCount = dxfEllipse.OwnerObjectSoftReference.Handle,
                        type = dxfEllipse.GetType().Name,
                        color = dxfEllipse.Color.Rgb,
                        transform = dxfEllipse.Transform.DebugString,

                        center = dxfEllipse.Center.ToString(),
                        majorAxisEndPoint = dxfEllipse.MajorAxisEndPoint.ToString(),
                        minorAxisEndPoint = dxfEllipse.MinorAxisEndPoint.ToString(),
                        startParameter = dxfEllipse.StartParameter,
                        endParameter = dxfEllipse.EndParameter

                    };

                    cadEntities.Add(cADEllipse);


                }



                if (typeof(DxfMText) == entityGroups.GetType())
                {
                    dxfMText = entityGroups as DxfMText;
                    // Console.WriteLine(dxfMText);

                    sw.Write(dxfMText.GetType());
                    sw.WriteLine();
                    sw.Write("文本:");
                    sw.Write(dxfMText.SimplifiedText);
                    sw.WriteLine();
                    sw.Write("文字大小:");
                    sw.Write(dxfMText.Height);
                    sw.WriteLine();
                    sw.Write("颜色:");
                    sw.Write(dxfMText.Color.Rgb);
                    sw.WriteLine();
                    sw.Write("变换矩阵:");
                    sw.Write(dxfMText.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();

                    if (dxfMText.SimplifiedText == "C座一层平面图")
                    {
                        //Console.WriteLine(1111111111111111111);
                        ktls.Add(dxfMText);


                    }


                    CADMText cADMText = new CADMText
                    {
                        calCount = dxfMText.OwnerObjectSoftReference.Handle,
                        type = dxfMText.GetType().Name,
                        color = dxfMText.Color.Rgb,
                        transform = dxfMText.Transform.DebugString,

                        simplifiedText = dxfMText.SimplifiedText.ToString(),
                        fontStyle = dxfMText.Style.ToString(),
                        size = dxfMText.Height,

                        attachmentPoint = dxfMText.AttachmentPoint.ToString(),
                        boxHeight = dxfMText.BoxHeight,
                        boxWidth = dxfMText.BoxWidth
                    };

                    cadEntities.Add(cADMText);

                    if (dxfMText.SimplifiedText.ToString() == "图 纸 目 录")
                    {
                        //Console.WriteLine(222222);
                        ktls.Add(dxfMText);

                    }
                    if (dxfMText.SimplifiedText.ToString() == "JS-T5-302")
                    {
                        //Console.WriteLine(222222);
                        ktls.Add(dxfMText);

                    }



                }


                if (typeof(DxfText) == entityGroups.GetType())
                {
                    dxfText = entityGroups as DxfText;
                    // Console.WriteLine(dxfText);

                    sw.Write(dxfText.GetType());
                    sw.WriteLine();
                    sw.Write("文本:");
                    sw.Write(dxfText.SimplifiedText);
                    sw.WriteLine();
                    sw.Write("文字大小:");
                    sw.Write(dxfText.Height);
                    sw.WriteLine();
                    sw.Write("颜色:");
                    sw.Write(dxfText.Color.Rgb);
                    sw.WriteLine();
                    sw.Write("变换矩阵:");
                    sw.Write(dxfText.Transform);
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();

                    CADText cADText = new CADText
                    {
                        calCount = dxfText.OwnerObjectSoftReference.Handle,
                        type = dxfText.GetType().Name,
                        color = dxfText.Color.Rgb,
                        transform = dxfText.Transform.DebugString,

                        simplifiedText = dxfText.SimplifiedText.ToString(),
                        fontStyle = dxfText.Style.ToString(),
                        size = dxfText.Height,

                        alignMentPoint1 = dxfText.AlignmentPoint1.ToString(),
                        rotationAngle = dxfText.Rotation,
                    };


                    if (dxfText.SimplifiedText == "JS-T5-001 ")
                    {
                        //Console.WriteLine(1111111111111111111);
                        ktest.Add(dxfText);


                    }

                    cadEntities.Add(cADText);

                }

                if (typeof(DxfInsert) == entityGroups.GetType())
                {
                    dxfInsert = entityGroups as DxfInsert;
                    //Console.WriteLine(dxfInsert.Block.Entities.Count+"");
                    //dxfEntityCollection = dxfInsert.Block.Entities as DxfEntityCollection;
                    if (dxfInsert.Block != null)
                    {
                        FindEntities(dxfInsert.Block.Entities);
                    }



                    //Console.WriteLine(dxfInsert.Block.Handle);
                    ulong count;
                    if (dxfInsert.Block != null)
                    {
                        count = dxfInsert.Block.Handle;

                    }
                    else {
                        count = 0;
                    }

                    CADInsert cADInsert = new CADInsert
                    {
                        calCount = count,
                        type = dxfInsert.GetType().Name,
                        transform = dxfInsert.Transform.DebugString,

                        insertPoint = dxfInsert.InsertionPoint.ToString(),
                        rotationAngle = dxfInsert.Rotation,
                    };


                    cadEntities.Add(cADInsert);



                    kins.Add(dxfInsert);
                    kco.Add(Entities);
                }



                if (typeof(DxfAttributeDefinition) == entityGroups.GetType())
                {
                    dxfAttributeDefinition = entityGroups as DxfAttributeDefinition;
                    //Console.WriteLine(dxfAttributeDefinition);

                }
            }

        }




        public static void Main(string[] args)
        {


            if (args.Length != 2)
            {
                args = new string[2];
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test1.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test2.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test3.dwg";
                args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test4.dwg";
                //args[1] = "D:\\C项目\\CadLCmd\\CadLCmd\\dwg\\test5Insert.dwg";
            }
            string format = args[0];
            string filename = args[1];

            DxfModel model = null;
            try
            {
                string extension = Path.GetExtension(filename);
                if (string.Compare(extension, ".dwg", true) == 0)
                {
                    model = DwgReader.Read(filename);
                }
                else
                {
                    model = DxfReader.Read(filename);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error occurred: " + e.Message);
                Environment.Exit(1);
            }

            //foreach (var entityGroups in model.Entities.GroupBy(a => a.GetType()))
            //{

            //    Console.WriteLine(entityGroups.GetType());

            //    //if (typeof(DxfLine) == entityGroups.Key)
            //    //{
            //    //    foreach (var item in entityGroups)
            //    //    {
            //    //        Console.WriteLine(item.Color);

            //    //    }
            //    //}

            //}
            //FileStream fs = new FileStream("D:\\C项目\\CadLCmd\\CadLCmd\\txt\\test1.txt", FileMode.Create);



            FindEntities(model.Entities);



            string json = JsonConvert.SerializeObject(cadEntities);
            File.WriteAllText("D:\\C项目\\CadLCmd\\CadLCmd\\txt\\model.json", json);
            File.WriteAllText("D:\\Project\\cadtest\\node_modules\\@cadTestUbim\\res\\data\\dxfdata.json", json);

            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();

            KTCodes = ktls.ToArray();

            KTest = ktest.ToArray();

            DxfType = dxfType.ToArray();

            KInsert = kins.ToArray();

            KEC = kco.ToArray();

            Console.ReadKey();
            string outfile = Path.GetDirectoryName(Path.GetFullPath(filename)) + "\\12";
            Stream stream;
            if (format == "pdf")
            {
                BoundsCalculator boundsCalculator = new BoundsCalculator();
                boundsCalculator.GetBounds(model);
                Bounds3D bounds = boundsCalculator.Bounds;
                PaperSize paperSize = PaperSizes.GetPaperSize(PaperKind.Letter);
                // Lengths in inches. 
                float pageWidth = (float)paperSize.Width / 100f;
                float pageHeight = (float)paperSize.Height / 100f;
                float margin = 0.5f;
                // Scale and transform such that its fits max width/height 
                // and the top left middle of the cad drawing will match the  
                // top middle of the pdf page. 
                // The transform transforms to pdf pixels.
                Matrix4D to2DTransform = DxfUtil.GetScaleTransform(
                    bounds.Corner1,
                    bounds.Corner2,
                    new Point3D(bounds.Center.X, bounds.Corner2.Y, 0d),
                    new Point3D(new Vector3D(margin, margin, 0d) * PdfExporter.InchToPixel),
                    new Point3D(new Vector3D(pageWidth - margin, pageHeight - margin, 0d) * PdfExporter.InchToPixel),
                    new Point3D(new Vector3D(pageWidth / 2d, pageHeight - margin, 0d) * PdfExporter.InchToPixel)
                );
                using (stream = File.Create(outfile + ".pdf"))
                {
                    PdfExporter pdfGraphics = new PdfExporter(stream);
                    pdfGraphics.DrawPage(
                        model,
                        GraphicsConfig.WhiteBackgroundCorrectForBackColor,
                        to2DTransform,
                        paperSize
                    );
                    pdfGraphics.EndDocument();
                }
            }
            else
            {
                GDIGraphics3D graphics = new GDIGraphics3D(GraphicsConfig.BlackBackgroundCorrectForBackColor);
                Size maxSize = new Size(500, 500);
                Bitmap bitmap =
                    ImageExporter.CreateAutoSizedBitmap(
                        model,
                        graphics,
                        Matrix4D.Identity,
                        System.Drawing.Color.Black,
                        maxSize
                    );
                switch (format)
                {
                    case "bmp":
                        using (stream = File.Create(outfile + ".bmp"))
                        {
                            ImageExporter.EncodeImageToBmp(bitmap, stream);
                        }
                        break;
                    case "gif":
                        using (stream = File.Create(outfile + ".gif"))
                        {
                            ImageExporter.EncodeImageToGif(bitmap, stream);
                        }
                        break;
                    case "tiff":
                        using (stream = File.Create(outfile + ".tiff"))
                        {
                            ImageExporter.EncodeImageToTiff(bitmap, stream);
                        }
                        break;
                    case "png":
                        using (stream = File.Create(outfile + ".png"))
                        {
                            ImageExporter.EncodeImageToPng(bitmap, stream);
                        }
                        break;
                    case "jpg":
                        using (stream = File.Create(outfile + ".jpg"))
                        {
                            ImageExporter.EncodeImageToJpeg(bitmap, stream);
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown format " + format + ".");
                        break;
                }
            }
        }
        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
    }
}

