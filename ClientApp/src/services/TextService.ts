export default class TextService {
  static formatDateFromString(dateStr: string | undefined): string {
    if (!dateStr) return '-';
    return this.formatDateTime(new Date(dateStr));
  }

  static formatDateTime(date: Date): string {
    return date.toLocaleString(undefined, {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
