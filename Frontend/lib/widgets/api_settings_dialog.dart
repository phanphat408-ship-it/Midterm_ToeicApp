import 'package:flutter/material.dart';
import '../services/api_service.dart';
import '../theme/app_theme.dart';

class ApiSettingsDialog extends StatefulWidget {
  final VoidCallback onSaved;

  const ApiSettingsDialog({super.key, required this.onSaved});

  @override
  State<ApiSettingsDialog> createState() => _ApiSettingsDialogState();
}

class _ApiSettingsDialogState extends State<ApiSettingsDialog> {
  late TextEditingController _urlController;
  String _statusMessage = '';
  bool _isTesting = false;

  final List<String> _presets = [
    'https://localhost:44312',
    'http://localhost:44312',
    'https://10.0.2.2:44312',
    'http://10.0.2.2:44312',
    'https://localhost:44311',
    'https://10.0.2.2:44311',
  ];

  @override
  void initState() {
    super.initState();
    _urlController = TextEditingController(text: ApiService().baseUrl);
  }

  @override
  void dispose() {
    _urlController.dispose();
    super.dispose();
  }

  Future<void> _testConnection() async {
    setState(() {
      _isTesting = true;
      _statusMessage = 'Đang kiểm tra kết nối...';
    });

    ApiService().setBaseUrl(_urlController.text);
    try {
      final exams = await ApiService().getAllExams();
      setState(() {
        _isTesting = false;
        _statusMessage = 'Kết nối thành công! Lấy được ${exams.length} đề thi.';
      });
    } catch (e) {
      setState(() {
        _isTesting = false;
        _statusMessage = 'Không thể kết nối. Sẽ dùng chế độ Offline/Fallback.';
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
      title: Row(
        children: [
          Container(
            padding: const EdgeInsets.all(8),
            decoration: BoxDecoration(
              color: AppColors.royalBlueSubtle,
              borderRadius: BorderRadius.circular(10),
            ),
            child: const Icon(Icons.settings_ethernet, color: AppColors.primaryRoyalBlue),
          ),
          const SizedBox(width: 12),
          const Text(
            'Cấu hình Backend API',
            style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
          ),
        ],
      ),
      content: SingleChildScrollView(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Địa chỉ REST API (.Web.Mvc hoặc .Web.Host):',
              style: TextStyle(fontSize: 13, color: AppColors.textMuted, fontWeight: FontWeight.w500),
            ),
            const SizedBox(height: 8),
            TextField(
              controller: _urlController,
              decoration: InputDecoration(
                hintText: 'https://localhost:44312',
                filled: true,
                fillColor: AppColors.surfaceMuted,
                border: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(12),
                  borderSide: const BorderSide(color: AppColors.border),
                ),
                contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
              ),
            ),
            const SizedBox(height: 12),
            const Text(
              'Chọn nhanh theo môi trường chạy:',
              style: TextStyle(fontSize: 12, fontWeight: FontWeight.bold, color: AppColors.textDark),
            ),
            const SizedBox(height: 6),
            Wrap(
              spacing: 6,
              runSpacing: 6,
              children: _presets.map((preset) {
                final isSelected = _urlController.text == preset;
                return ChoiceChip(
                  label: Text(preset, style: const TextStyle(fontSize: 11)),
                  selected: isSelected,
                  selectedColor: AppColors.primaryRoyalBlue,
                  labelStyle: TextStyle(
                    color: isSelected ? Colors.white : AppColors.textDark,
                    fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                  ),
                  onSelected: (selected) {
                    if (selected) {
                      setState(() {
                        _urlController.text = preset;
                      });
                    }
                  },
                );
              }).toList(),
            ),
            if (_statusMessage.isNotEmpty) ...[
              const SizedBox(height: 12),
              Container(
                padding: const EdgeInsets.all(10),
                decoration: BoxDecoration(
                  color: _statusMessage.contains('thành công')
                      ? AppColors.emeraldLight
                      : AppColors.redLight,
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Text(
                  _statusMessage,
                  style: TextStyle(
                    fontSize: 12,
                    fontWeight: FontWeight.w600,
                    color: _statusMessage.contains('thành công')
                        ? AppColors.emeraldDark
                        : AppColors.redError,
                  ),
                ),
              ),
            ],
          ],
        ),
      ),
      actions: [
        TextButton(
          onPressed: _isTesting ? null : _testConnection,
          child: _isTesting
              ? const SizedBox(width: 16, height: 16, child: CircularProgressIndicator(strokeWidth: 2))
              : const Text('Kiểm tra kết nối'),
        ),
        ElevatedButton(
          onPressed: () {
            ApiService().setBaseUrl(_urlController.text);
            widget.onSaved();
            Navigator.of(context).pop();
          },
          style: ElevatedButton.styleFrom(
            backgroundColor: AppColors.primaryRoyalBlue,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          ),
          child: const Text('Lưu & Áp dụng'),
        ),
      ],
    );
  }
}
