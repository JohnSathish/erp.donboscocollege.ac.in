import { Injectable } from '@angular/core';

export enum LogLevel {
  Debug = 0,
  Info = 1,
  Warn = 2,
  Error = 3,
}

@Injectable({ providedIn: 'root' })
export class LoggingService {
  private readonly minLevel: LogLevel = LogLevel.Debug; // Can be configured via environment

  debug(message: string, ...args: any[]): void {
    if (this.shouldLog(LogLevel.Debug)) {
      console.debug(`[DEBUG] ${message}`, ...args);
    }
  }

  info(message: string, ...args: any[]): void {
    if (this.shouldLog(LogLevel.Info)) {
      console.info(`[INFO] ${message}`, ...args);
    }
  }

  warn(message: string, ...args: any[]): void {
    if (this.shouldLog(LogLevel.Warn)) {
      console.warn(`[WARN] ${message}`, ...args);
    }
  }

  error(message: string, error?: any, ...args: any[]): void {
    if (this.shouldLog(LogLevel.Error)) {
      console.error(`[ERROR] ${message}`, error, ...args);
    }
  }

  private shouldLog(level: LogLevel): boolean {
    return level >= this.minLevel;
  }
}
