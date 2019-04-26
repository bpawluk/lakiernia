using iTextSharp.text;
using iTextSharp.text.pdf;
using Lakiernia.Model;
using Lakiernia.Data_Access;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lakiernia.Utils
{
    public class GeneratorFaktur
    {
        public static void generuj(Zamowienie zamowienie)
        {
            if (!Directory.Exists("Faktury")) Directory.CreateDirectory("Faktury");
            String fileName = @"Faktury\Zamowienie nr " + zamowienie.Klient.ID + "-" + zamowienie.ID + "-" + zamowienie.DataZlozenia.Month +
                                 zamowienie.DataOdbioru.Day + ".pdf";
            FileStream fs;
            try
            {
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            } catch
            {
                MessageBox.Show("Błąd dostępu do pliku. Zamknij niepotrzebne pliki.");
                return;
            }
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            Klient klient = zamowienie.Klient;
            Pracodawca pracodawca;
            using (PracodawcaDAO bd = new PracodawcaDAO()) pracodawca = bd.Pobierz().FirstOrDefault();

            BaseFont font = BaseFont.CreateFont(@"Fonts\calibri.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font naglowek = new Font(font, 18);
            Font tytul = new Font(font, 14);
            Font tresc = new Font(font, 10);
            Font tabela = new Font(font, 8);

            PdfPTable daneNaglowek = new PdfPTable(2);
            daneNaglowek.WidthPercentage = 100;
            daneNaglowek.AddCell(komorka("Zleceniobiorca", PdfPCell.ALIGN_LEFT, tytul));
            daneNaglowek.AddCell(komorka("Zleceniodawca", PdfPCell.ALIGN_LEFT, tytul));

            PdfPTable dane = new PdfPTable(new float[] { 1, 4, 1, 4 });
            dane.WidthPercentage = 100;
            dane.AddCell(komorka("Firma:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Firma, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Nazwa:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(klient.Nazwa, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Adres:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Ulica + " " + pracodawca.Numer, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Adres:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(klient.Ulica + " " + klient.Numer, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Miasto + " " + pracodawca.Kod, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(klient.Miasto + " " + klient.Kod, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("NIP:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Nip, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("NIP:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(klient.Nip, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Telefon:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Telefon, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Email:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Email, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Bank:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Bank, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("Nr. konta:", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka(pracodawca.Konto, PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));
            dane.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tresc));

            PdfPTable pozycje = new PdfPTable(new float[] { 1, 3, 3, 2, 2, 1, 2, 2, 2, 2, 3, 3, 3 });
            pozycje.WidthPercentage = 100;
            pozycje.AddCell(komorkaObrys("Lp.", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Materiał", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Kolor", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Dł.", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Szer.", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Szt.", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Pow.", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Cena netto", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Rabat", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("VAT", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Kwota netto", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Kwota VAT", PdfPCell.ALIGN_CENTER, tabela, true));
            pozycje.AddCell(komorkaObrys("Kwota brutto", PdfPCell.ALIGN_CENTER, tabela, true));
            int i = 1;
            decimal sumaNetto = 0;
            decimal sumaVat = 0;
            decimal sumaBrutto = 0;
            foreach (Pozycja p in zamowienie.Pozycje)
            {
                Material material;
                if ((material = p.Material) != null)
                {
                    decimal pow = p.Liczba * (decimal)material.Dlugosc / 1000 * (decimal)material.Szerokosc / 1000;
                    decimal netto = Math.Round(pow * p.Cena - (decimal)p.Rabat/100M * pow * p.Cena, 2);
                    decimal vat = Math.Round(netto * p.VAT/100, 2);
                    decimal brutto = netto + vat;
                    sumaNetto += netto;
                    sumaVat += vat;
                    sumaBrutto += brutto;
                    pozycje.AddCell(komorkaObrys(i.ToString(), PdfPCell.ALIGN_CENTER, tabela));
                    pozycje.AddCell(komorkaObrys(material.Nazwa, PdfPCell.ALIGN_LEFT, tabela));
                    pozycje.AddCell(komorkaObrys(p.Farba.Kolor + " " + p.Farba.Producent, PdfPCell.ALIGN_LEFT, tabela));
                    pozycje.AddCell(komorkaObrys(material.Dlugosc + " mm", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys(material.Szerokosc + " mm", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys(p.Liczba.ToString(), PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys($"{pow:F2} m²", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys(p.Cena.ToString() + " zł/m²", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys(p.Rabat.ToString() + " %", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys(p.VAT.ToString() + " %", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys($"{netto:F2} zł", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys($"{vat:F2} zł", PdfPCell.ALIGN_RIGHT, tabela));
                    pozycje.AddCell(komorkaObrys($"{brutto:F2} zł", PdfPCell.ALIGN_RIGHT, tabela));
                    i++;
                }
            }
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_CENTER, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_LEFT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorka("", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorkaObrys("Suma:", PdfPCell.ALIGN_LEFT, tabela, true));
            pozycje.AddCell(komorkaObrys($"{sumaNetto:F2} zł", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorkaObrys($"{sumaVat:F2} zł", PdfPCell.ALIGN_RIGHT, tabela));
            pozycje.AddCell(komorkaObrys($"{sumaBrutto:F2} zł", PdfPCell.ALIGN_RIGHT, tabela));

            Paragraph naglowekP = new Paragraph(@"Zamówienie nr " + zamowienie.Klient.ID + "/" + zamowienie.ID + "/" +
                                                  zamowienie.DataZlozenia.Month + zamowienie.DataOdbioru.Day, naglowek);
            naglowekP.Alignment = Element.ALIGN_CENTER;
            Paragraph dataP = new Paragraph(@"Wystawiono dnia  " + DateTime.Now.ToShortDateString(), tresc);
            dataP.SpacingAfter = 5;
            Paragraph pozycjeTytulP = new Paragraph(@"Materiały pomalowane w ramach zlecenia", tytul);
            pozycjeTytulP.SpacingAfter = 5;
            Paragraph podsumowanieP = new Paragraph($"Razem do zapłaty: {sumaBrutto:F2} zł", tytul);
            podsumowanieP.Alignment = Element.ALIGN_RIGHT;

            doc.Open();
            doc.Add(naglowekP);
            doc.Add(Chunk.NEWLINE);
            doc.Add(dataP);
            doc.Add(daneNaglowek);
            doc.Add(dane);
            doc.Add(Chunk.NEWLINE);            
            doc.Add(pozycjeTytulP);
            doc.Add(pozycje);
            doc.Add(Chunk.NEWLINE);
            doc.Add(podsumowanieP);
            doc.Close();

            System.Diagnostics.Process.Start(fileName);
        }

        public static PdfPCell komorka(String text, int alignment, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Padding = 2;
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = PdfPCell.NO_BORDER;
            return cell;
        }

        public static PdfPCell komorkaObrys(String text, int alignment, Font font, bool naglowek = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            if (naglowek) cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.Padding = 2;
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
    }
}