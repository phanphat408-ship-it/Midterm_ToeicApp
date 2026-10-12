import 'package:flutter/material.dart';
import '../models/exam_models.dart';
import '../models/submit_models.dart';
import '../theme/app_theme.dart';

class ExamResultScreen extends StatefulWidget {
  final SubmitExamResult result;
  final String examTitle;
  final List<QuestionItem> questions;

  const ExamResultScreen({
    super.key,
    required this.result,
    required this.examTitle,
    required this.questions,
  });

  @override
  State<ExamResultScreen> createState() => _ExamResultScreenState();
}

class _ExamResultScreenState extends State<ExamResultScreen> {
  String _filter = 'All'; // 'All', 'Wrong', 'Skipped'

  List<QuestionResultItem> get _filteredDetails {
    if (_filter == 'Wrong') {
      return widget.result.details.where((d) => !d.isCorrect && (d.selectedOption != null && d.selectedOption!.isNotEmpty)).toList();
    } else if (_filter == 'Skipped') {
      return widget.result.details.where((d) => d.selectedOption == null || d.selectedOption!.isEmpty).toList();
    }
    return widget.result.details;
  }

  String _formatDuration() {
    if (widget.result.startTime == null || widget.result.submitTime == null) {
      return '42m 15s';
    }
    final diff = widget.result.submitTime!.difference(widget.result.startTime!);
    final m = diff.inMinutes;
    final s = diff.inSeconds % 60;
    return '${m}m ${s}s';
  }

  @override
  Widget build(BuildContext context) {
    final res = widget.result;
    double percentage = res.totalQuestions > 0 ? (res.totalCorrect / res.totalQuestions) * 100 : 0;

    return Scaffold(
      backgroundColor: AppColors.background,
      appBar: AppBar(
        title: const Text('Exam results'),
        backgroundColor: AppColors.primaryRoyalBlue,
        automaticallyImplyLeading: false,
        actions: [
          IconButton(
            icon: const Icon(Icons.close_rounded),
            onPressed: () => Navigator.of(context).popUntil((route) => route.isFirst),
            tooltip: 'Về trang chủ',
          ),
        ],
      ),
      body: SingleChildScrollView(
        child: Column(
          children: [
            // Top Score Card
            _buildScoreHero(percentage),
            const SizedBox(height: 16),
            // 3 Stat Boxes
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16),
              child: Row(
                children: [
                  Expanded(child: _buildStatBox('Correct', '${res.totalCorrect}', AppColors.emeraldLight, AppColors.emeraldDark, Icons.check_circle_rounded)),
                  const SizedBox(width: 10),
                  Expanded(child: _buildStatBox('Wrong', '${res.totalWrong}', AppColors.redLight, AppColors.redError, Icons.cancel_rounded)),
                  const SizedBox(width: 10),
                  Expanded(child: _buildStatBox('Skipped', '${res.totalSkipped}', AppColors.surfaceMuted, AppColors.textMuted, Icons.remove_circle_outline_rounded)),
                ],
              ),
            ),
            const SizedBox(height: 24),
            // Answer Review Section
            _buildReviewSection(),
          ],
        ),
      ),
      bottomNavigationBar: Container(
        padding: const EdgeInsets.all(16),
        decoration: const BoxDecoration(
          color: Colors.white,
          border: Border(top: BorderSide(color: AppColors.border)),
        ),
        child: SafeArea(
          top: false,
          child: ElevatedButton(
            onPressed: () => Navigator.of(context).popUntil((route) => route.isFirst),
            style: ElevatedButton.styleFrom(
              backgroundColor: AppColors.primaryRoyalBlue,
              padding: const EdgeInsets.symmetric(vertical: 14),
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            ),
            child: const Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Icon(Icons.home_rounded, size: 20),
                SizedBox(width: 8),
                Text('Back to Home', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold)),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildScoreHero(double percentage) {
    return Container(
      width: double.infinity,
      decoration: const BoxDecoration(
        color: AppColors.primaryRoyalBlue,
        borderRadius: BorderRadius.vertical(bottom: Radius.circular(32)),
      ),
      padding: const EdgeInsets.fromLTRB(20, 8, 20, 28),
      child: Column(
        children: [
          Text(
            widget.examTitle,
            textAlign: TextAlign.center,
            style: const TextStyle(
              color: Color(0xFFBAE6FD),
              fontSize: 14,
              fontWeight: FontWeight.w500,
            ),
          ),
          const SizedBox(height: 18),
          // Circle Score
          Container(
            width: 130,
            height: 130,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: Colors.white,
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withValues(alpha: 0.12),
                  blurRadius: 16,
                  offset: const Offset(0, 6),
                ),
              ],
            ),
            alignment: Alignment.center,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  '${widget.result.totalCorrect} / ${widget.result.totalQuestions}',
                  style: const TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.bold,
                    color: AppColors.primaryRoyalBlue,
                    letterSpacing: -0.5,
                  ),
                ),
                Text(
                  '${percentage.toStringAsFixed(1)}%',
                  style: const TextStyle(
                    fontSize: 13,
                    fontWeight: FontWeight.bold,
                    color: AppColors.emeraldGreen,
                  ),
                ),
                Text(
                  'Score: ${widget.result.score}/495',
                  style: const TextStyle(
                    fontSize: 10,
                    fontWeight: FontWeight.w600,
                    color: AppColors.textMuted,
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(height: 14),
          Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.timer_outlined, size: 14, color: Color(0xFFBAE6FD)),
              const SizedBox(width: 4),
              Text(
                'Completed in ${_formatDuration()}',
                style: const TextStyle(color: Colors.white, fontSize: 13, fontWeight: FontWeight.w500),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStatBox(String label, String value, Color bgColor, Color textColor, IconData icon) {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 14, horizontal: 10),
      decoration: BoxDecoration(
        color: bgColor,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: textColor.withValues(alpha: 0.2)),
      ),
      child: Column(
        children: [
          Icon(icon, color: textColor, size: 22),
          const SizedBox(height: 6),
          Text(
            value,
            style: TextStyle(
              fontSize: 18,
              fontWeight: FontWeight.bold,
              color: textColor,
            ),
          ),
          const SizedBox(height: 2),
          Text(
            label,
            style: TextStyle(
              fontSize: 12,
              fontWeight: FontWeight.w600,
              color: textColor.withValues(alpha: 0.85),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildReviewSection() {
    final details = _filteredDetails;

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text(
                'Answer review',
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                  color: AppColors.textDark,
                  letterSpacing: -0.3,
                ),
              ),
              Text(
                '${details.length} questions',
                style: const TextStyle(fontSize: 13, color: AppColors.textMuted),
              ),
            ],
          ),
          const SizedBox(height: 12),
          // Filter Chips
          Row(
            children: [
              _buildReviewFilterChip('All', '${widget.result.details.length}'),
              const SizedBox(width: 8),
              _buildReviewFilterChip('Wrong', '${widget.result.totalWrong}'),
              const SizedBox(width: 8),
              _buildReviewFilterChip('Skipped', '${widget.result.totalSkipped}'),
            ],
          ),
          const SizedBox(height: 16),
          // List of questions review
          ListView.builder(
            shrinkWrap: true,
            physics: const NeverScrollableScrollPhysics(),
            itemCount: details.length,
            itemBuilder: (context, index) {
              return _buildReviewItem(details[index]);
            },
          ),
        ],
      ),
    );
  }

  Widget _buildReviewFilterChip(String label, String count) {
    final isSelected = _filter == label;
    return GestureDetector(
      onTap: () => setState(() => _filter = label),
      child: Container(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 6),
        decoration: BoxDecoration(
          color: isSelected ? AppColors.primaryRoyalBlue : Colors.white,
          borderRadius: BorderRadius.circular(20),
          border: Border.all(
            color: isSelected ? AppColors.primaryRoyalBlue : AppColors.border,
          ),
        ),
        child: Text(
          '$label ($count)',
          style: TextStyle(
            fontSize: 12,
            fontWeight: isSelected ? FontWeight.bold : FontWeight.w500,
            color: isSelected ? Colors.white : AppColors.textDark,
          ),
        ),
      ),
    );
  }

  Widget _buildReviewItem(QuestionResultItem item) {
    final isCorrect = item.isCorrect;
    final isSkipped = item.selectedOption == null || item.selectedOption!.isEmpty;

    Color badgeColor = isCorrect
        ? AppColors.emeraldGreen
        : isSkipped
            ? AppColors.textMuted
            : AppColors.redError;

    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: AppColors.border),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Row(
                children: [
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                    decoration: BoxDecoration(
                      color: AppColors.surfaceMuted,
                      borderRadius: BorderRadius.circular(6),
                    ),
                    child: Text(
                      'Câu ${item.questionNumber}',
                      style: const TextStyle(fontSize: 12, fontWeight: FontWeight.bold, color: AppColors.textDark),
                    ),
                  ),
                  const SizedBox(width: 8),
                  if (item.questionText != null && item.questionText!.isNotEmpty)
                    ConstrainedBox(
                      constraints: const BoxConstraints(maxWidth: 180),
                      child: Text(
                        item.questionText!,
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                        style: const TextStyle(fontSize: 12, color: AppColors.textMuted),
                      ),
                    ),
                ],
              ),
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                decoration: BoxDecoration(
                  color: badgeColor.withValues(alpha: 0.12),
                  borderRadius: BorderRadius.circular(6),
                ),
                child: Row(
                  children: [
                    Icon(
                      isCorrect
                          ? Icons.check_circle_outline
                          : isSkipped
                              ? Icons.remove_circle_outline
                              : Icons.cancel_outlined,
                      size: 14,
                      color: badgeColor,
                    ),
                    const SizedBox(width: 4),
                    Text(
                      isCorrect
                          ? 'Correct'
                          : isSkipped
                              ? 'Skipped'
                              : 'Wrong',
                      style: TextStyle(
                        fontSize: 11,
                        fontWeight: FontWeight.bold,
                        color: badgeColor,
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
          const SizedBox(height: 12),
          // User answer vs Correct answer row
          Container(
            padding: const EdgeInsets.all(10),
            decoration: BoxDecoration(
              color: AppColors.surfaceMuted,
              borderRadius: BorderRadius.circular(10),
            ),
            child: Row(
              children: [
                Expanded(
                  child: Row(
                    children: [
                      const Text(
                        'Your answer: ',
                        style: TextStyle(fontSize: 12, color: AppColors.textMuted),
                      ),
                      Text(
                        isSkipped ? 'None' : '(${item.selectedOption})',
                        style: TextStyle(
                          fontSize: 13,
                          fontWeight: FontWeight.bold,
                          color: isCorrect
                              ? AppColors.emeraldDark
                              : isSkipped
                                  ? AppColors.textMuted
                                  : AppColors.redError,
                        ),
                      ),
                    ],
                  ),
                ),
                Container(height: 16, width: 1, color: AppColors.border),
                const SizedBox(width: 12),
                Expanded(
                  child: Row(
                    children: [
                      const Text(
                        'Correct answer: ',
                        style: TextStyle(fontSize: 12, color: AppColors.textMuted),
                      ),
                      Text(
                        '(${item.correctOption})',
                        style: const TextStyle(
                          fontSize: 13,
                          fontWeight: FontWeight.bold,
                          color: AppColors.emeraldDark,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
