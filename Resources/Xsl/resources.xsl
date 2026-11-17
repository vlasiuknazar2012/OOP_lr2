<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="html" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ресурси">
    <html>
      <head>
        <title>Інформаційні ресурси кафедри</title>
        <style>
          body { font-family: Arial, sans-serif; margin: 16px; }
          table { border-collapse: collapse; width: 100%; }
          th, td { border: 1px solid #888; padding: 6px 8px; text-align: left; vertical-align: top; }
          th { background-color: #e6e6e6; }
        </style>
      </head>
      <body>
        <h1>Інформаційні ресурси кафедри</h1>
        <table>
          <tr>
            <th>№</th>
            <th>Назва ресурсу</th>
            <th>Вид</th>
            <th>Анотація</th>
            <th>Автор (ПІП)</th>
            <th>Факультет</th>
            <th>Кафедра</th>
            <th>Умови використання</th>
            <th>Адреса</th>
          </tr>
          <xsl:for-each select="ресурс">
            <tr>
              <td><xsl:value-of select="position()"/></td>
              <td><xsl:value-of select="@назва"/></td>
              <td><xsl:value-of select="@вид"/></td>
              <td><xsl:value-of select="@анотація"/></td>
              <td><xsl:value-of select="автор/@ПІП"/></td>
              <td><xsl:value-of select="автор/@факультет"/></td>
              <td><xsl:value-of select="автор/@кафедра"/></td>
              <td><xsl:value-of select="@умови"/></td>
              <td><xsl:value-of select="@адреса"/></td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>
