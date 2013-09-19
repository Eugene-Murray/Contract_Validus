<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:res="urn:Microsoft.Search.Response" xmlns:doc="urn:Microsoft.Search.Response.Document" xmlns:search="urn:Microsoft.Search.Response.Document.Document">
  <xsl:output method="html" indent="yes"/>
  <!-- Main body template. Sets the Results view (Relevance or date) options -->
  <xsl:template name="dvt_1.body">
    <div class="val-padding">
      <xsl:apply-templates/>
    </div>
  </xsl:template>
  <xsl:template match="res:Response">
    <xsl:apply-templates select="res:Range"/>
  </xsl:template>
  <xsl:template match="res:Range">
    <xsl:variable name="StartAt" select="res:StartAt"/>
    <xsl:variable name="ACount" select="res:Count"/>
    <xsl:variable name="TotalAvailable" select="res:TotalAvailable"/>
    <xsl:variable name="Take" select="res:Take"/>
    <xsl:variable name="ToPos">
      <xsl:if test="($StartAt + ($Take - 1)) >= $TotalAvailable">
        <xsl:value-of select="$TotalAvailable"/>
      </xsl:if>
      <xsl:if test="($StartAt + ($Take - 1)) &lt; $TotalAvailable">
        <xsl:value-of select="($StartAt + ($Take - 1))"/>
      </xsl:if>
    </xsl:variable>
    <xsl:variable name="CurrentPos">
      <xsl:value-of select="($StartAt + ($Take - 1)) div $Take"/>
    </xsl:variable>
    <xsl:variable name="MaxPos">
      <xsl:if test="($TotalAvailable mod $Take) > 0">
        <xsl:value-of select="floor($TotalAvailable div $Take) + 1"/>
      </xsl:if>
      <xsl:if test="($TotalAvailable mod $Take) = 0">
        <xsl:value-of select="floor($TotalAvailable div $Take)"/>
      </xsl:if>
    </xsl:variable>
    <xsl:apply-templates select="res:Results"/>
    <div class="row-fluid">
      <div class="span7 dataTables_info">
        <xsl:value-of select="$StartAt"/> to <xsl:value-of select="$ToPos"/> of <xsl:value-of select="$TotalAvailable"/>
      </div>
      <div class="span5">
        <div class="pagination pagination-mini">
          <ul>
            <xsl:call-template name="GenPrev">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
            </xsl:call-template>
            <xsl:call-template name="GenPage1">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
              <xsl:with-param name="MaxPos" select="$MaxPos"/>
            </xsl:call-template>
            <xsl:call-template name="GenPage2">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
              <xsl:with-param name="MaxPos" select="$MaxPos"/>
            </xsl:call-template>
            <xsl:call-template name="GenPage3">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
              <xsl:with-param name="MaxPos" select="$MaxPos"/>
            </xsl:call-template>
            <xsl:call-template name="GenPage4">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
              <xsl:with-param name="MaxPos" select="$MaxPos"/>
            </xsl:call-template>
            <xsl:call-template name="GenPage5">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
              <xsl:with-param name="MaxPos" select="$MaxPos"/>
            </xsl:call-template>
            <xsl:call-template name="GenNext">
              <xsl:with-param name="CurrentPos" select="$CurrentPos"/>
              <xsl:with-param name="MaxPos" select="$MaxPos"/>
            </xsl:call-template>
          </ul>
        </div>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="res:Results">
    <xsl:apply-templates select="doc:Document"/>
  </xsl:template>
  <xsl:template match="doc:Document">
    <xsl:apply-templates select="search:Properties"/>
  </xsl:template>
  <xsl:template match="search:Properties">
    <xsl:variable name="cType">
      <xsl:value-of select="search:Property/search:Name[. ='VALContentType']/following-sibling::search:Value"/>
    </xsl:variable>
    <xsl:if test="$cType = 'Policy' or $cType = 'Claims' or $cType = 'Broker' or $cType = 'Insured'">
      <div class="row-fluid">
        <div class="span12">
          <div class="val-xslt-spacer">
            <xsl:call-template name="VALResults">
              <xsl:with-param name="cType" select="$cType"/>
            </xsl:call-template>
          </div>
        </div>
      </div>
    </xsl:if>
  </xsl:template>
  <xsl:template name="VALResults">
    <xsl:param name="cType"/>
    <xsl:choose>
      <xsl:when test="$cType = 'Policy'">
        <xsl:variable name="PolId">
          <xsl:value-of select="search:Property/search:Name[. ='VALPolID']/following-sibling::search:Value"/>
        </xsl:variable>
        <xsl:call-template name="VALHeader">
          <xsl:with-param name="cType" select="$cType"/>
          <xsl:with-param name="cTitle" select="$PolId"/>
        </xsl:call-template>
        <table cellpadding="0" cellspacing="0" border="0" style="border-collapse:collapse;">
          <tbody>
            <xsl:call-template name="Policy"/>
          </tbody>
        </table>
      </xsl:when>
      <xsl:when test="$cType = 'Claims'">
        <xsl:variable name="BPR">
          <xsl:value-of select="search:Property/search:Name[. ='VALBpr']/following-sibling::search:Value"/>
        </xsl:variable>
        <xsl:call-template name="VALHeader">
          <xsl:with-param name="cType" select="$cType"/>
          <xsl:with-param name="cTitle" select="$BPR"/>
        </xsl:call-template>
        <table cellpadding="0" cellspacing="0" border="0" style="border-collapse:collapse;">
          <tbody>
            <xsl:call-template name="CMSClaims"/>
          </tbody>
        </table>
      </xsl:when>
      <xsl:when test="$cType = 'Broker'">
        <xsl:variable name="Broker">
          <xsl:value-of select="search:Property/search:Name[. ='title']/following-sibling::search:Value"/>
        </xsl:variable>
        <xsl:call-template name="VALHeader">
          <xsl:with-param name="cType" select="$cType"/>
          <xsl:with-param name="cTitle" select="$Broker"/>
        </xsl:call-template>
        <table cellpadding="0" cellspacing="0" border="0" style="border-collapse:collapse;">
          <tbody>
            <xsl:call-template name="Broker"/>
          </tbody>
        </table>
      </xsl:when>
      <xsl:when test="$cType = 'Insured'">
        <xsl:variable name="InsdNm">
          <xsl:value-of select="search:Property/search:Name[. ='VALInsdNm']/following-sibling::search:Value"/>
        </xsl:variable>
        <xsl:call-template name="VALHeader">
          <xsl:with-param name="cType" select="$cType"/>
          <xsl:with-param name="cTitle" select="$InsdNm"/>
        </xsl:call-template>
        <table cellpadding="0" cellspacing="0" border="0" style="border-collapse:collapse;">
          <tbody>
            <xsl:call-template name="Insured"/>
          </tbody>
        </table>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="Insured">
    <xsl:variable name="InsdName" select="search:Property/search:Name[. ='VALInsdNm']/following-sibling::search:Value"/>
    <xsl:variable name="SynEPI" select="search:Property/search:Name[. ='VALSyndicateEPI']/following-sibling::search:Value"/>
    <xsl:variable name="USMPremium" select="search:Property/search:Name[. ='VALUSMPremium']/following-sibling::search:Value"/>
    <xsl:variable name="GrossSynEPI" select="search:Property/search:Name[. ='VALGrossSyndEPI']/following-sibling::search:Value"/>
    <xsl:variable name="GrossIncLossRatio" select="search:Property/search:Name[. ='VALGrossIncLossRatio']/following-sibling::search:Value"/>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Syndicate EPI</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>USM Premium</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Gross Syndicate EPI</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Gross Inc Loss Ratio</xsl:text>
      </td>
      <td/>
    </tr>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;border-left:1px solid #73AEE6;">
        <xsl:value-of select="$SynEPI"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$USMPremium"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$GrossSynEPI"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$GrossIncLossRatio"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px;"/>
    </tr>
  </xsl:template>
  <xsl:template name="Broker">
    <xsl:variable name="Broker" select="search:Property/search:Name[. ='Broker']/following-sibling::search:Value"/>
    <xsl:variable name="SynEPI" select="search:Property/search:Name[. ='VALSyndicateEPI']/following-sibling::search:Value"/>
    <xsl:variable name="USMPremium" select="search:Property/search:Name[. ='VALUSMPremium']/following-sibling::search:Value"/>
    <xsl:variable name="GrossSynEPI" select="search:Property/search:Name[. ='VALGrossSyndEPI']/following-sibling::search:Value"/>
    <xsl:variable name="GrossIncLossRatio" select="search:Property/search:Name[. ='VALGrossIncLossRatio']/following-sibling::search:Value"/>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Syndicate EPI</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>USM Premium</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Gross Syndicate EPI</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Gross Inc Loss Ratio</xsl:text>
      </td>
      <td/>
    </tr>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;border-left:1px solid #73AEE6;">
        <xsl:value-of select="$SynEPI"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$USMPremium"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$GrossSynEPI"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$GrossIncLossRatio"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px;"/>
    </tr>
  </xsl:template>
  <xsl:template name="CMSClaims">
    <xsl:variable name="PolId" select="search:Property/search:Name[. ='VALPolID']/following-sibling::search:Value"/>
    <xsl:variable name="Bpr" select="search:Property/search:Name[. ='VALBpr']/following-sibling::search:Value"/>
    <xsl:variable name="Reserve" select="search:Property/search:Name[. ='VALReserve']/following-sibling::search:Value"/>
    <xsl:variable name="Paid" select="search:Property/search:Name[. ='VALPaid']/following-sibling::search:Value"/>
    <xsl:variable name="Description" select="search:Property/search:Name[. ='VALDescription']/following-sibling::search:Value"/>
    <xsl:variable name="DOL" select="search:Property/search:Name[. ='VALDol']/following-sibling::search:Value"/>
    <xsl:variable name="BkrCtc" select="search:Property/search:Name[. ='VALBkrCtc']/following-sibling::search:Value"/>
    <xsl:variable name="St" select="search:Property/search:Name[. ='VALSt']/following-sibling::search:Value"/>
    <xsl:variable name="Ldr" select="search:Property/search:Name[. ='VALLdr']/following-sibling::search:Value"/>
    <xsl:variable name="CreationDt" select="search:Property/search:Name[. ='VALCreationDt']/following-sibling::search:Value"/>
    <xsl:variable name="LossLocn" select="search:Property/search:Name[. ='VALLossLocn']/following-sibling::search:Value"/>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>BPR</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Policy Id</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>LdrCd</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Status</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>DOL</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Creation Dt</xsl:text>
      </td>
      <td/>
    </tr>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;border-left:1px solid #73AEE6;">
        <xsl:value-of select="$Bpr"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <a>
          <xsl:attribute name="href">
            <xsl:text>http://webpolicy.talbotdev.com/webpolicy.aspx?policyId=</xsl:text>
            <xsl:value-of select="$PolId"/>
          </xsl:attribute>
          <xsl:attribute name="target">
            <xsl:text>_blank</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="$PolId"/>
          </xsl:attribute>
          <xsl:value-of select="$PolId"/>
        </a>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$Ldr"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$St"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$DOL"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$CreationDt"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px;"/>
    </tr>
  </xsl:template>
  <xsl:template name="PolicyHeader">
    <xsl:variable name="PolId" select="search:Property/search:Name[. ='VALPolID']/following-sibling::search:Value"/>
    <div class="Title">
      <a/>
    </div>
    <p>Policy</p>
  </xsl:template>
  <xsl:template name="Policy">
    <xsl:variable name="PolId" select="search:Property/search:Name[. ='VALPolID']/following-sibling::search:Value"/>
    <xsl:variable name="InsdName" select="search:Property/search:Name[. ='VALInsdNm']/following-sibling::search:Value"/>
    <xsl:variable name="AcctgYr" select="search:Property/search:Name[. ='VALAcctgYr']/following-sibling::search:Value"/>
    <xsl:variable name="BkrPsu" select="search:Property/search:Name[. ='VALBkrCd']/following-sibling::search:Value"/>
    <xsl:variable name="Division" select="search:Property/search:Name[. ='VALDivision']/following-sibling::search:Value"/>
    <xsl:variable name="EntSt" select="search:Property/search:Name[. ='VALEntSt']/following-sibling::search:Value"/>
    <xsl:variable name="IncpDt" select="search:Property/search:Name[. ='VALIncpDt']/following-sibling::search:Value"/>
    <xsl:variable name="Ldr" select="search:Property/search:Name[. ='VALLdr']/following-sibling::search:Value"/>
    <xsl:variable name="OrigOff" select="search:Property/search:Name[. ='VALOrigOff']/following-sibling::search:Value"/>
    <xsl:variable name="St" select="search:Property/search:Name[. ='VALSt']/following-sibling::search:Value"/>
    <xsl:variable name="UwrPsu" select="search:Property/search:Name[. ='VALUwrPsu']/following-sibling::search:Value"/>
    <xsl:variable name="WtnDt" select="search:Property/search:Name[. ='VALWtnDt']/following-sibling::search:Value"/>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Insured</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Type</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Ent St</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Status</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Uwr</xsl:text>
      </td>
      <td/>
      <td style="border-width:0;color:#444444;font-weight:bold;padding:0px 4px;">
        <xsl:text>Ldr</xsl:text>
      </td>
      <td/>
    </tr>
    <tr style="color:#FFFFFF;font-size:8pt;text-align:left;font-family:Tahoma">
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;border-left:1px solid #73AEE6;">
        <xsl:value-of select="$InsdName"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:text/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$EntSt"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$St"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$UwrPsu"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px"/>
      <td style="border-bottom:1px solid #73AEE6;color:#000000;font-weight:normal;padding:0px 4px;">
        <xsl:value-of select="$Ldr"/>
      </td>
      <td style="border-bottom:1px solid #73AEE6;font-size:1pt;padding:0px 4px;width:8px;"/>
    </tr>
  </xsl:template>
  <xsl:template name="VALHeader">
    <xsl:param name="cType"/>
    <xsl:param name="cTitle"/>

    <xsl:choose>
      <xsl:when test="$cType='Policy'">
        <a>
          <xsl:attribute name="href">
            <xsl:text>http://webpolicy.talbotdev.com/webpolicy.aspx?policyId=</xsl:text>
            <xsl:value-of select="$cTitle"/>
          </xsl:attribute>
          <xsl:attribute name="target">
            <xsl:text>_blank</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="$cTitle"/>
          </xsl:attribute>
          <xsl:value-of select="$cTitle"/>
          <xsl:text> (Subscribe) </xsl:text>
        </a>
      </xsl:when>
      <xsl:when test="$cType='Claims'">
        <a>
          <xsl:attribute name="href">
            <xsl:text>http://intra.talbotdev.com/apps/cms/claimDetails.jsp?bpr=</xsl:text>
            <xsl:value-of select="$cTitle"/>
          </xsl:attribute>
          <xsl:attribute name="target">
            <xsl:text>_blank</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="title">
            <xsl:value-of select="$cTitle"/>
          </xsl:attribute>
          <xsl:value-of select="$cTitle"/>
          <xsl:text> (CMS) </xsl:text>
        </a>
      </xsl:when>
      <xsl:when test="$cType='Broker'">
        <xsl:value-of select="$cTitle"/>
        <xsl:text> (Broker Measures) </xsl:text>
      </xsl:when>
      <xsl:when test="$cType='Insured'">
        <xsl:value-of select="$cTitle"/>
        <xsl:text> (Insured Measures) </xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="GenPrev">
    <xsl:param name="CurrentPos"/>
    <xsl:if test="$CurrentPos > 1">
      <li class="prev">
        <a href="#">? Previous</a>
      </li>
    </xsl:if>
    <xsl:if test="$CurrentPos = 1">
      <li class="prev disabled">
        <a href="#">? Previous</a>
      </li>
    </xsl:if>
  </xsl:template>
  <xsl:template name="GenNext">
    <xsl:param name="CurrentPos"/>
    <xsl:param name="MaxPos"/>
    <xsl:if test="$CurrentPos &lt; $MaxPos">
      <li class="next">
        <a href="#">Next ? </a>
      </li>
    </xsl:if>
    <xsl:if test="$CurrentPos = $MaxPos">
      <li class="next disabled">
        <a href="#">Next ? </a>
      </li>
    </xsl:if>
  </xsl:template>
  <xsl:template name="GenPage1">
    <xsl:param name="CurrentPos"/>
    <xsl:param name="MaxPos"/>
    <xsl:if test="$CurrentPos &lt; 4">
      <xsl:if test="$CurrentPos = 1">
        <li class="active">
          <a href="#">1</a>
        </li>
      </xsl:if>
      <xsl:if test="$CurrentPos > 1">
        <li>
          <a href="#">1</a>
        </li>
      </xsl:if>
    </xsl:if>
    <xsl:if test="$CurrentPos >= 4">
      <xsl:if test="$CurrentPos > ( $MaxPos -2 )">
        <li>
          <a href="#">
            <xsl:value-of select="$MaxPos -4"/>
          </a>
        </li>
      </xsl:if>
      <xsl:if test="$CurrentPos &lt;= ( $MaxPos -2 )">
        <li>
          <a href="#">
            <xsl:value-of select="$CurrentPos - 2"/>
          </a>
        </li>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template name="GenPage2">
    <xsl:param name="CurrentPos"/>
    <xsl:param name="MaxPos"/>
    <xsl:if test="$CurrentPos &lt; 4">
      <xsl:choose>
        <xsl:when test="$CurrentPos = 2 and $MaxPos >= 2">
          <li class="active">
            <a href="#">2</a>
          </li>
        </xsl:when>
        <xsl:when test="$CurrentPos != 2 and $MaxPos >= 2">
          <li>
            <a href="#">2</a>
          </li>
        </xsl:when>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$CurrentPos >= 4">
      <xsl:if test="$CurrentPos > ( $MaxPos -2 )">
        <li>
          <a href="#">
            <xsl:value-of select="$MaxPos -3"/>
          </a>
        </li>
      </xsl:if>
      <xsl:if test="$CurrentPos &lt;= ( $MaxPos -2 )">
        <li>
          <a href="#">
            <xsl:value-of select="$CurrentPos - 1"/>
          </a>
        </li>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template name="GenPage3">
    <xsl:param name="CurrentPos"/>
    <xsl:param name="MaxPos"/>
    <xsl:if test="$CurrentPos &lt; 4">
      <xsl:choose>
        <xsl:when test="$CurrentPos = 3 and $MaxPos >= 3">
          <li class="active">
            <a href="#">3</a>
          </li>
        </xsl:when>
        <xsl:when test="$CurrentPos != 3 and $MaxPos >= 3">
          <li>
            <a href="#">3</a>
          </li>
        </xsl:when>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="$CurrentPos >= 4">
      <xsl:if test="$CurrentPos > ( $MaxPos -2 )">
        <li>
          <a href="#">
            <xsl:value-of select="$MaxPos -2"/>
          </a>
        </li>
      </xsl:if>
      <xsl:if test="$CurrentPos &lt;= ( $MaxPos -2 )">
        <li class="active">
          <a href="#">
            <xsl:value-of select="$CurrentPos"/>
          </a>
        </li>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template name="GenPage4">
    <xsl:param name="CurrentPos"/>
    <xsl:param name="MaxPos"/>
    <xsl:choose>
      <xsl:when test="$CurrentPos &lt; 4 and $MaxPos >=4">
        <li>
          <a href="#">4</a>
        </li>
      </xsl:when>
      <xsl:when test="$CurrentPos &lt; 4 and $MaxPos &lt; 4">
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$CurrentPos = ( $MaxPos -1 )">
            <li class="active">
              <a href="#">
                <xsl:value-of select="$MaxPos -1"/>
              </a>
            </li>
          </xsl:when>
          <xsl:when test="$CurrentPos > ( $MaxPos -1 )">
            <li>
              <a href="#">
                <xsl:value-of select="$MaxPos -1"/>
              </a>
            </li>
          </xsl:when>
          <xsl:otherwise>
            <li>
              <a href="#">
                <xsl:value-of select="$CurrentPos + 1"/>
              </a>
            </li>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="GenPage5">
    <xsl:param name="CurrentPos"/>
    <xsl:param name="MaxPos"/>
    <xsl:choose>
      <xsl:when test="$CurrentPos &lt; 4 and $MaxPos >=5">
        <li>
          <a href="#">5</a>
        </li>
      </xsl:when>
      <xsl:when test="$CurrentPos &lt; 4 and $MaxPos &lt; 5">
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$CurrentPos = $MaxPos ">
            <li class="active">
              <a href="#">
                <xsl:value-of select="$MaxPos"/>
              </a>
            </li>
          </xsl:when>
          <xsl:when test="$CurrentPos = ($MaxPos - 1) ">
            <li>
              <a href="#">
                <xsl:value-of select="$MaxPos"/>
              </a>
            </li>
          </xsl:when>
          <xsl:otherwise>
            <li>
              <a href="#">
                <xsl:value-of select="$CurrentPos + 2"/>
              </a>
            </li>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- XSL transformation starts here -->
  <xsl:template match="/">
    <xsl:call-template name="dvt_1.body"/>
  </xsl:template>
</xsl:stylesheet>
