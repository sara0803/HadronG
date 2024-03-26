using System;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using UnityEngine;
using TextAlignment = iText.Layout.Properties.TextAlignment;

namespace ReportPdf
{
   public class CreateReportPDF : MonoBehaviour
   {
      [SerializeField]
      private Texture2D renderTexture;
      
      private void Awake()
      {
         var tmpBytes = renderTexture.GetRawTextureData();
         var tmpPath = Application.dataPath + "/demo.pdf";
         Debug.Log("tmpPath = " + tmpPath);
         var tmpUri = new Uri(@"E:\ImportantProject\azminigames\Assets\VisualArt\SpritesUI\RelojArenaAnimation\RelojArena_00113.png");
         ImageData tmpImageData = ImageDataFactory.Create(tmpUri);
         PdfWriter tmpPdfWriter = new PdfWriter(tmpPath);
         PdfDocument tmpPdfDocument = new PdfDocument(tmpPdfWriter);
         Document tmpDocument = new Document(tmpPdfDocument);
         Paragraph tmpHeader = new Paragraph("Header").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20);
         Image tmpImage = new Image(tmpImageData).ScaleAbsolute(100, 200).SetFixedPosition(0, 0);
         tmpDocument.Add(tmpHeader);
         tmpDocument.Add(tmpImage);
         tmpDocument.Close();
      }
   }
}