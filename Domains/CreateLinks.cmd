@echo off

@rem Base

rmdir base\Workspace\Typescript\Domain\src\allors\meta\base
rmdir base\Workspace\Typescript\Angular\src\allors\meta\base
rmdir base\Workspace\Typescript\Angular\src\allors\domain\base
rmdir base\Workspace\Typescript\Material\src\allors\meta\base
rmdir base\Workspace\Typescript\Material\src\allors\domain\base
rmdir base\Workspace\Typescript\Material\src\allors\angular\base

mklink /D base\Workspace\Typescript\Domain\src\allors\meta\base base\Workspace\Typescript\Meta\src\allors\meta\base
mklink /D base\Workspace\Typescript\Angular\src\allors\meta\base base\Workspace\Typescript\Meta\src\allors\meta\base
mklink /D base\Workspace\Typescript\Angular\src\allors\domain\base base\Workspace\Typescript\Domain\src\allors\domain\base
mklink /D base\Workspace\Typescript\Material\src\allors\meta\base base\Workspace\Typescript\Meta\src\allors\meta\base
mklink /D base\Workspace\Typescript\Material\src\allors\domain\base base\Workspace\Typescript\Domain\src\allors\domain\base
mklink /D base\Workspace\Typescript\Material\src\allors\angular\base base\Workspace\Typescript\Angular\src\allors\angular\base

@rem Apps

rmdir apps\Workspace\Typescript\Angular\src\allors\meta\base
rmdir apps\Workspace\Typescript\Angular\src\allors\domain\base
rmdir apps\Workspace\Typescript\Angular\src\allors\angular\base
rmdir apps\Workspace\Typescript\Angular\src\allors\material\base

mklink /D apps\Workspace\Typescript\Angular\src\allors\meta\base base\Workspace\Typescript\Meta\src\allors\meta\base
mklink /D apps\Workspace\Typescript\Angular\src\allors\domain\base base\Workspace\Typescript\Domain\src\allors\domain\base
mklink /D apps\Workspace\Typescript\Angular\src\allors\angular\base base\Workspace\Typescript\Angular\src\allors\angular\base
mklink /D apps\Workspace\Typescript\Angular\src\allors\material\base base\Workspace\Typescript\Material\src\allors\material\base


