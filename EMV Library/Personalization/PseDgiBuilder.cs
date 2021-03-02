using System;
using System.Collections.Generic;
using System.Linq;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Personalization
{
public class PseDgiBuilder
{
	private readonly PsePersonalizationData _data;
	private readonly PsePersonalizationModel _model;

	#region >> Constructors

	/// <summary>
	/// Initializes a new instance.
	/// </summary>
	/// <param name="model">PSE DGI model.</param>
	/// <param name="data">PSE data.</param>
	public PseDgiBuilder(PsePersonalizationModel model, PsePersonalizationData data)
	{
		this._data = data;
		this._model = model;
	}

	#endregion

	/// <summary>
	/// Builds DGI to be used with STORE DATA command for given records having the same index.
	/// </summary>
	/// <param name="sfi"></param>
	/// <param name="index"></param>
	/// <param name="records"></param>
	/// <returns></returns>
	public string BuildDgi(byte sfi, byte index, IEnumerable<PseRecord> records)
	{
		var tlv70 = new TlvData(0x70, new List<TlvData>());

		foreach (var record in records)
		{
			var tlvs = new List<TlvData> {
				new TlvData { Tag = 0x4F, Value = record.AdfName.FromHexa() },
				new TlvData { Tag = 0x50, Value = record.ApplicationLabel.FromString() }
			};

			if (!String.IsNullOrWhiteSpace(record.PreferredName))
			{
				tlvs.Add(new TlvData { Tag = 0x9F12, Value = record.PreferredName.FromString() });
			}

			if (record.PriorityIndicator.HasValue)
			{
				tlvs.Add(new TlvData { Tag = 0x87, Value = record.PriorityIndicator.Value.ToByteArray() });
			}

			if (!String.IsNullOrWhiteSpace(record.DiscretionaryData))
			{
				tlvs.Add(new TlvData { Tag = 0x73, Value = record.DiscretionaryData.FromHexa() });
			}

			var tlv61 = new TlvData(0x61, tlvs);
			tlv70.InnerTlvs.Add(tlv61);
		}

		var dgiLength = TlvDataHelper.ToBerEncodedL((uint)tlv70.Length / 2);

		return $"{sfi:X2}{index:X2}{dgiLength.ToHexa('\0')}{tlv70.ToByteArray().ToHexa()}";
	}

	/// <summary>
	/// Builds DGI to be used with STORE DATA command for given FCI .
	/// </summary>
	/// <param name="fciModel"></param>
	/// <returns></returns>
	public string BuildDgi(FciModel fciModel)
	{
		var fields = new List<TagModel>();

		if (fciModel.Tags != null)
		{
			fields.AddRange(fciModel.Tags);
		}

		var tagModel = new TagModel { Tag = "A5", Fields = fields };
		var command = BuildDgi(tagModel);

		return $"{fciModel.Dgi}{TlvDataHelper.ToBerEncodedL((uint)command.Length / 2).ToHexa('\0')}{command}";
	}

	/// <summary>
	/// Builds DGI to be used with STORE DATA command for given tag.
	/// </summary>
	/// <param name="tagModel"></param>
	/// <returns></returns>
	private string BuildDgi(TagModel tagModel)
	{
		return BuildTlv(tagModel)
			.ToByteArray()
			.ToHexa('\0');
	}

	private TlvData BuildTlv(TagModel tagModel)
	{
		var tlv = new TlvData { Tag = Convert.ToUInt32(tagModel.Tag, 16) };

		if (tagModel.Fields == null)
		{
			tlv.Value = tagModel.Tag switch
			{
				// Language Preference
				"5F2D" => _data.UnmanagedAttributes[tagModel.Tag]
						.ToObject<string[]>()
						.Aggregate(String.Empty, (c, l) => c + l)
						.FromString(),
				_ => _data.UnmanagedAttributes[tagModel.Tag]
						.ToObject<string>()
						.FromHexa()
			};
		}
		else
		{
			tlv.InnerTlvs = tagModel.Fields.Select(BuildTlv).ToList();
		}

		return tlv;
	}
}}
